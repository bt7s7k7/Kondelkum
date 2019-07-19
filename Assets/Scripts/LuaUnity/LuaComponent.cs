using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLua;

namespace LuaUnity {
	public class LuaComponent : MonoBehaviour {

		[System.Serializable]
		public struct ImportedObject {
			public string label;
			public Object target;
		}

		public LuaState state;
		[Multiline]
		public string code;
		public ImportedObject[] importedObjects;

		public object FindObject(string name) {
			foreach (var imported in importedObjects) {
				if (imported.label == name) {
					return imported.target;
				}
			}
			return null;
		}

		[B.MethodButton("Run")]
		public void Run() {
			var lua = LuaState.ReadOrGlobal(state);
			lua["this"] = this;
			lua.DoString(code);
			lua["this"] = null;
		}

		public void Run(int value) {
			var lua = LuaState.ReadOrGlobal(state);
			lua["arg"] = value;
			Run();
			lua["arg"] = null;
		}
		public void Run(float value) {
			var lua = LuaState.ReadOrGlobal(state);
			lua["arg"] = value;
			Run();
			lua["arg"] = null;
		}
		public void Run(bool value) {
			var lua = LuaState.ReadOrGlobal(state);
			lua["arg"] = value;
			Run();
			lua["arg"] = null;
		}
		public void Run(string value) {
			var lua = LuaState.ReadOrGlobal(state);
			lua["arg"] = value;
			Run();
			lua["arg"] = null;
		}
	}
}