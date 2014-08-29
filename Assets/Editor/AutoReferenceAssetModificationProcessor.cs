using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public class AutoReferenceAssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
	public static string[] OnWillSaveAssets(string[] paths)
	{
		var components = GameObject.FindObjectsOfType<MonoBehaviour>();
		var allGO = new List<GameObject>(GameObject.FindObjectsOfType<GameObject>());

		foreach (var c in components)
		{
			var fields = c.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			foreach (var f in fields)
			{
				string alias;
				if (TryGetReferenceName(f, out alias))
				{
					var children = allGO.FindAll(g => g.name == alias && g.transform.IsChildOf(c.transform));

					if (f.FieldType == typeof(GameObject))
					{
						Assign(c, f, alias, children);
					}
					else
					{
						var candidates = children.ConvertAll(go => go.GetComponent(f.FieldType))
							.FindAll(instance => instance != null);
						Assign(c, f, alias, candidates);
					}
				}
			}
		}
		return paths;
	}

	static bool TryGetReferenceName(FieldInfo fieldInfo, out string name)
	{
		var attributes = fieldInfo.GetCustomAttributes(typeof(AutoReference), true) as AutoReference[];
		if (attributes.Length > 0)
		{
			name = attributes[0].Alias;

			if (string.IsNullOrEmpty(name))
				name = fieldInfo.Name;

			return true;
		}
		else
		{
			name = null;
			return false;
		}
	}

	static bool Assign<T>(Component c, FieldInfo f, string alias, List<T> list) where T : Object
	{
		if (list.Count == 1)
		{
			f.SetValue(c, list[0]);
			return true;
		}
		else if (list.Count == 0)
		{
			Debug.LogError(string.Format("{0}.{1} is missing (can't find \"{2}({3})\" under \"{4}\")", c.GetType().Name, f.Name, alias, f.FieldType, c.name));
		}
		else
		{
			Debug.LogError(string.Format("{0}.{1} is duplicated ({2} \"{3}\" under \"{4}\")", c.GetType().Name, f.Name, list.Count, alias, c.name));
		}
		return false;
	}
}
