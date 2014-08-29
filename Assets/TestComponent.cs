using UnityEngine;
using System.Collections;

public class TestComponent : MonoBehaviour
{
	[AutoReference]
	public GameObject MyGameObject;

	[AutoReference]
	public SampleComponent MySample;


	[AutoReference("MyGameObject_Alias")]
	public GameObject MyGameObject1;

	[AutoReference("MySample_Alias")]
	public SampleComponent MySample2;

	[AutoReference("DUPLICATE")]
	public SampleComponent MySample_DUP;

	[AutoReference("NOT_FOUND")]
	public SampleComponent MySample_MISSING;
}
