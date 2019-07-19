using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLua;

namespace LuaUnity {

	public class LuaState : MonoBehaviour {
		public static Lua global;

		Lua state;
		public Lua Get() {
			if (state == null) {
				state = new Lua();
				state.LoadCLRPackage();
				state.DoString("import ('UnityEngine')");
			}
			return state;
		}


		public static Lua ReadOrGlobal(LuaState state) {
			if (state == null) {
				if (global == null) {
					global = new Lua();
					global.LoadCLRPackage();
					global.DoString("import ('UnityEngine')");
				}
				return global;
			} else {
				return state.Get();
			}

		}
	}
}