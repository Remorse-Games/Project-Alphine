using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Remorse.Tools.RPGDatabase.Utility;

namespace Remorse.Tools.RPGDatabase.Window
{
    public class TypeTab : BaseTab
    {
        //Having list of all elements exist in data.
        public List<TypeElementData> element = new List<TypeElementData>();
        //Having list of all skills exist in data.
        public List<TypeSkillData> skill = new List<TypeSkillData>();
        //Having list of all weapons exist in data.
        public List<TypeWeaponData> weapon = new List<TypeWeaponData>();
        //Having list of all armors exist in data.
        public List<TypeArmorData> armor = new List<TypeArmorData>();
        //Having list of all equipmentType exist in data.
        public List<TypeEquipmentData> equipment = new List<TypeEquipmentData>();

        public List<string> elementDisplayName = new List<string>();
        public List<string> skillDisplayName = new List<string>();
        public List<string> weaponDisplayName = new List<string>();
        public List<string> armorDisplayName = new List<string>();
        public List<string> equipmentDisplayName = new List<string>();

        //All GUIStyle variable initialization.
        GUIStyle typeStyle;
        GUIStyle tabStyle;
        GUIStyle columnStyle;

        #region  DeleteLater

        //How many type in ChangeMaximum Func
        public int elementSize;
        public int elementSizeTemp;
        public int skillSize;
        public int skillSizeTemp;
        public int weaponSize;
        public int weaponSizeTemp;
        public int armorSize;
        public int armorSizeTemp;
        public int equipmentSize;
        public int equipmentSizeTemp;

        //i don't know about this but i leave this to handle later.
        int elementIndex = 0;
        int elementIndexTemp = -1;
        int skillIndex = 0;
        int skillIndexTemp = -1;
        int weaponIndex = 0;
        int weaponIndexTemp = -1;
        int armorIndex = 0;
        int armorIndexTemp = -1;
        int equipmentIndex = 0;
        int equipmentIndexTemp = -1;

        //Scroll position. Is this necessary?
        Vector2 elementScrollPos = Vector2.zero;
        Vector2 skillScrollPos = Vector2.zero;
        Vector2 weaponScrollPos = Vector2.zero;
        Vector2 armorScrollPos = Vector2.zero;
        Vector2 equipmentScrollPos = Vector2.zero;
        #endregion

        public void Init()
        {
            element.Clear();
            skill.Clear();
            weapon.Clear();
            armor.Clear();
            equipment.Clear();
            LoadGameData<TypeElementData>(ref elementSize, element, PathDatabase.ElementRelativeDataPath);
            ElementListReset();
            LoadGameData<TypeSkillData>(ref skillSize, skill, PathDatabase.SkillRelativeDataPath);
            SkillListReset();
            LoadGameData<TypeWeaponData>(ref weaponSize, weapon, PathDatabase.WeaponRelativeDataPath);
            WeaponListReset();
            LoadGameData<TypeArmorData>(ref armorSize, armor, PathDatabase.ArmorRelativeDataPath);
            ArmorListReset();
            LoadGameData<TypeEquipmentData>(ref equipmentSize, equipment, PathDatabase.EquipmentRelativeDataPath);
            EquipmentListReset();
        }


        public void OnRender(Rect position)
        {
            #region A Bit Explanation About Local Tab
            ///So there is 2 types of Tab,
            ///One is in Database that not included here.
            ///Second, there is 3 tab slicing in typeTab itself.
            ///So make sure you understand that tabbing in here does not
            ///have any corelation with DatabaseMain.cs Tab system.
            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////START REGION OF VALUE INIT/////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////

            float tabWidth = position.width * .85f;
            float tabHeight = position.height - 10f;

            float eachTabWidth = tabWidth * .195f;

            //Style area.
            typeStyle = new GUIStyle(GUI.skin.box);
            typeStyle.normal.background = CreateTexture(1, 1, Color.gray);
            columnStyle = new GUIStyle(GUI.skin.box);
            columnStyle.normal.background = CreateTexture(1, 1, new Color32(99, 100, 100, 200));
            tabStyle = new GUIStyle(GUI.skin.box);
            if (EditorGUIUtility.isProSkin)
                tabStyle.normal.background = CreateTexture(1, 1, new Color32(76, 76, 76, 200));
            else
                tabStyle.normal.background = CreateTexture(1, 1, new Color32(200, 200, 200, 200));

            ////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////END REGION OF VALUE INIT///////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////

            #region Entry Of typeTab GUILayout
            //Start drawing the whole typeTab.
            GUILayout.BeginArea(new Rect(position.width / 7, 5, tabWidth, tabHeight));

            //The black box behind the typeTab? yes, this one.
            GUILayout.Box(" ", typeStyle, GUILayout.Width(position.width - DatabaseMain.tabAreaWidth), GUILayout.Height(position.height - 25f));


            #region Tab 1/5
            //First Tab of five
            GUILayout.BeginArea(new Rect(0, 0, tabWidth, tabHeight));
            GUILayout.Box("Elements", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            elementScrollPos = GUILayout.BeginScrollView(elementScrollPos, false, true, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .79f));
            elementIndex = GUILayout.SelectionGrid(
                elementIndex,
                elementDisplayName.ToArray(),
                1,
                GUILayout.Width(eachTabWidth - 20),
                GUILayout.Height(position.height / 24 * elementSize
                ));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && elementIndex != elementIndexTemp)
            {
                elementIndexTemp = elementIndex;
                ItemTabLoader(elementIndexTemp);
                elementIndexTemp = -1;
            }
            //Text field to change each index name
            if (elementSize > 0)
            {
                element[elementIndex].dataName = GUILayout.TextField(element[elementIndex].dataName, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                elementDisplayName[elementIndex] = element[elementIndex].dataName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            }
            //Int field of change Maximum
            elementSizeTemp = EditorGUILayoutExt.IntField(0, 999, elementSizeTemp, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                elementIndex = 0;
                elementSize = elementSizeTemp;
                ChangeMaximum<TypeElementData>(elementSize, element, PathDatabase.ElementExplicitDataPath);
                ElementListReset();
            }
            else if (elementSizeTemp <= 0)
            {
                elementSizeTemp = elementSize;
            }
            GUILayout.EndArea();
            #endregion  

            #region Tab 2/5
            //Second Tab of five
            GUILayout.BeginArea(new Rect(eachTabWidth + 5, 0, tabWidth, tabHeight));
            GUILayout.Box("Skill Types", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            skillScrollPos = GUILayout.BeginScrollView(skillScrollPos, false, true, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .79f));
            skillIndex = GUILayout.SelectionGrid(
                skillIndex,
                skillDisplayName.ToArray(),
                1,
                GUILayout.Width(eachTabWidth - 20),
                GUILayout.Height(position.height / 24 * skillSize
                ));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && skillIndex != skillIndexTemp)
            {
                skillIndexTemp = skillIndex;
                ItemTabLoader(skillIndexTemp);
                skillIndexTemp = -1;
            }
            //Text field to change each index name
            if (skillSize > 0)
            {
                skill[skillIndex].dataName = GUILayout.TextField(skill[skillIndex].dataName, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                skillDisplayName[skillIndex] = skill[skillIndex].dataName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            }
            //Int field of change Maximum
            skillSizeTemp = EditorGUILayoutExt.IntField(0, 999, skillSizeTemp, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                skillIndex = 0;
                skillSize = skillSizeTemp;
                ChangeMaximum<TypeSkillData>(skillSize, skill, PathDatabase.SkillExplicitDataPath);
                SkillListReset();
            }
            else if (skillSizeTemp <= 0)
            {
                skillSizeTemp = skillSize;
            }
            GUILayout.EndArea();
            #endregion  

            #region Tab 3/5
            //Third Tab of five
            GUILayout.BeginArea(new Rect(2 * eachTabWidth + 10, 0, tabWidth, tabHeight));
            GUILayout.Box("Weapon Types", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            weaponScrollPos = GUILayout.BeginScrollView(weaponScrollPos, false, true, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .79f));
            weaponIndex = GUILayout.SelectionGrid(
                weaponIndex,
                weaponDisplayName.ToArray(),
                1,
                GUILayout.Width(eachTabWidth - 20),
                GUILayout.Height(position.height / 24 * weaponSize
                ));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && weaponIndex != weaponIndexTemp)
            {
                weaponIndexTemp = weaponIndex;
                ItemTabLoader(weaponIndexTemp);
                weaponIndexTemp = -1;
            }
            //Text field to change each index name
            if (weaponSize > 0)
            {
                weapon[weaponIndex].dataName = GUILayout.TextField(weapon[weaponIndex].dataName, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                weaponDisplayName[weaponIndex] = weapon[weaponIndex].dataName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            }
            //Int field of change Maximum
            weaponSizeTemp = EditorGUILayoutExt.IntField(0, 999, weaponSizeTemp, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                weaponIndex = 0;
                weaponSize = weaponSizeTemp;
                ChangeMaximum<TypeWeaponData>(weaponSize, weapon, PathDatabase.WeaponExplicitDataPath);
                WeaponListReset();
            }
            else if (weaponSizeTemp <= 0)
            {
                weaponSizeTemp = weaponSize;
            }
            GUILayout.EndArea();
            #endregion

            #region Tab 4/5
            //Forth Tab of five
            GUILayout.BeginArea(new Rect(3 * eachTabWidth + 15, 0, tabWidth, tabHeight));
            GUILayout.Box("Armor Types", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            armorScrollPos = GUILayout.BeginScrollView(armorScrollPos, false, true, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .79f));
            armorIndex = GUILayout.SelectionGrid(
                armorIndex,
                armorDisplayName.ToArray(),
                1,
                GUILayout.Width(eachTabWidth - 20),
                GUILayout.Height(position.height / 24 * armorSize
                ));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && armorIndex != armorIndexTemp)
            {
                armorIndexTemp = armorIndex;
                ItemTabLoader(armorIndexTemp);
                armorIndexTemp = -1;
            }
            //Text field to change each index name
            if (armorSize > 0)
            {
                armor[armorIndex].dataName = GUILayout.TextField(armor[armorIndex].dataName, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                armorDisplayName[armorIndex] = armor[armorIndex].dataName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            }
            //Int field of change Maximum
            armorSizeTemp = EditorGUILayoutExt.IntField(0, 999, armorSizeTemp, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                armorIndex = 0;
                armorSize = armorSizeTemp;
                ChangeMaximum<TypeArmorData>(armorSize, armor, PathDatabase.ArmorExplicitDataPath);
                ArmorListReset();
            }
            else if (armorSizeTemp <= 0)
            {
                armorSizeTemp = armorSize;
            }
            GUILayout.EndArea();
            #endregion  

            #region Tab 5/5
            //Last Tab
            GUILayout.BeginArea(new Rect(4 * eachTabWidth + 20, 0, tabWidth, tabHeight));
            GUILayout.Box("Equipment Types", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15));

            //Scroll View
            #region ScrollView
            equipmentScrollPos = GUILayout.BeginScrollView(equipmentScrollPos, false, true, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .79f));
            equipmentIndex = GUILayout.SelectionGrid(
                equipmentIndex,
                equipmentDisplayName.ToArray(),
                1,
                GUILayout.Width(eachTabWidth - 20),
                GUILayout.Height(position.height / 24 * equipmentSize
                ));
            GUILayout.EndScrollView();
            #endregion

            //Happen everytime selection grid is updated
            if (GUI.changed && equipmentIndex != equipmentIndexTemp)
            {
                equipmentIndexTemp = equipmentIndex;
                ItemTabLoader(equipmentIndexTemp);
                equipmentIndexTemp = -1;
            }
            //Text field to change each index name
            if (equipmentSize > 0)
            {
                equipment[equipmentIndex].dataName = GUILayout.TextField(equipment[equipmentIndex].dataName, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
                equipmentDisplayName[equipmentIndex] = equipment[equipmentIndex].dataName;
            }
            else
            {
                GUILayout.TextField("Null", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            }
            //Int field of change Maximum
            equipmentSizeTemp = EditorGUILayoutExt.IntField(0, 999, equipmentSizeTemp, GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10));
            if (GUILayout.Button("Change Maximum", GUILayout.Width(eachTabWidth), GUILayout.Height(position.height * .75f / 15 - 10)))
            {
                equipmentIndex = 0;
                equipmentSize = equipmentSizeTemp;
                ChangeMaximum<TypeEquipmentData>(equipmentSize, equipment, PathDatabase.EquipmentExplicitDataPath);
                EquipmentListReset();
            }
            else if (equipmentSizeTemp <= 0)
            {
                equipmentSizeTemp = equipmentSize;
            }
            GUILayout.EndArea();
            #endregion


            GUILayout.EndArea();
            #endregion

            //Setting Dirty
            EditorUtility.SetDirty(element[elementIndex]);
            EditorUtility.SetDirty(skill[skillIndex]);
            EditorUtility.SetDirty(weapon[weaponIndex]);
            EditorUtility.SetDirty(armor[armorIndex]);
            EditorUtility.SetDirty(equipment[equipmentIndex]);
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void ElementListReset()
        {
            elementDisplayName.Clear();
            for (int i = 0; i < elementSize; i++)
            {
                elementDisplayName.Add(element[i].dataName);
            }
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void SkillListReset()
        {
            skillDisplayName.Clear();
            for (int i = 0; i < skillSize; i++)
            {
                skillDisplayName.Add(skill[i].dataName);
            }
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void WeaponListReset()
        {
            weaponDisplayName.Clear();
            for (int i = 0; i < weaponSize; i++)
            {
                weaponDisplayName.Add(weapon[i].dataName);
            }
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void ArmorListReset()
        {
            armorDisplayName.Clear();
            for (int i = 0; i < armorSize; i++)
            {
                armorDisplayName.Add(armor[i].dataName);
            }
        }

        ///<summary>
        ///Clears out the displayName list and add it with new value
        ///</summary>
        private void EquipmentListReset()
        {
            equipmentDisplayName.Clear();
            for (int i = 0; i < equipmentSize; i++)
            {
                equipmentDisplayName.Add(equipment[i].dataName);
            }
        }

        public override void ItemTabLoader(int index)
        {

        }
    }
}
