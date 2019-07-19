using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New level", menuName ="Level", order =100000)]
public class LevelPrototype : ScriptableObject {
	public string customLabel;
	public string number;
	public LevelPrototype[] requirements;
	public SceneReference scene;
	[Multiline]
	public string desc;
}
