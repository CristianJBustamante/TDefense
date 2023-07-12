 using UnityEditor.UI;
 using UnityEditor;

 namespace com.Pizia.TouchJoypadSystem
 {
     [CustomEditor (typeof (TouchJoypadButton))]
     public class TouchJoypadButtonEditor : SelectableEditor
     {
         public override void OnInspectorGUI ()
         {
             TouchJoypadButton targetMenuButton = (TouchJoypadButton) target;

             targetMenuButton.buttonName = EditorGUILayout.TextField ("Button Name", targetMenuButton.buttonName);
             targetMenuButton.unpressedByMoveFinger = EditorGUILayout.Toggle ("Exit Releases", targetMenuButton.unpressedByMoveFinger);

             // Show default inspector property editor
             base.OnInspectorGUI ();
         }
     }
 }