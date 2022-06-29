using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComponentPool
{
    // Edit group category here
    #region Component Group Enum

    [System.Serializable]
    public enum EComponentGroup
    {
        None,
        Script = 1,
        DisplayUI = 2,
        DiscribeUI = 3,
        FunctionUI = 4,
        TeleportSpot = 5,
    }

    #endregion

    #region Staff<T> Class

    [System.Serializable]
    public class Staff<T>
    {
        [SerializeField]
        private string staffName;
        [SerializeField]
        private T component;

        public string StaffName => staffName;
        public T Component => component;

        public Staff()
        {
            component = default(T);
            staffName = string.Empty;
        }

        public Staff(T component, string staffName)
        {
            this.component = component;
            this.staffName = staffName;
        }

        public void SetComponent(T component)
        {
            this.component = component;
        }
    }

    #endregion

    public static class Components
    {
        #region Staff Class

        [System.Serializable]
        public class Staff
        {
            [SerializeField]
            private string staffName;
            [SerializeField]
            private Component component;

            public string StaffName => staffName;
            public Component Component => component;

            public Staff()
            {
                component = null;
                staffName = string.Empty;
            }

            public Staff(Component component, string staffName)
            {
                this.component = component;
                this.staffName = staffName;
            }
        }

        #endregion

        #region Staff Group Class

        [System.Serializable]
        public class StaffGroup
        {
            [SerializeField] private EComponentGroup groupName;

            [SerializeField] private List<Staff> staffList;

            #region Reachable Properties

            public EComponentGroup GroupName => groupName;

            public List<Staff> StaffList => staffList;

            public Staff this[string staffName]
            {
                get
                {
                    var staff = staffList.Where(s => s.StaffName == staffName);

                    return staff.ToArray().Length <= 0 ? null : staff.First();
                }
            }

            #endregion

            public StaffGroup()
            {
                groupName = EComponentGroup.None;

                staffList = new List<Staff>();
            }

            public StaffGroup(EComponentGroup state)
            {
                groupName = state;

                staffList = new List<Staff>();
            }

            public bool Add(Staff staff)
            {
                if (staffList.Count <= 0) { staffList.Add(staff); return true; }

                if (this[staff.StaffName] != null) { return false; }

                staffList.Add(staff);
                staffList.Sort(delegate (Staff x, Staff y) { return x.StaffName.CompareTo(y.StaffName); });

                return true;
            }

            public bool Remove(string staffName)
            {
                if (staffList.Count <= 0) { return false; }

                var staff = this[staffName];

                return staff == null ? false : staffList.Remove(staff);
            }
        }

        #endregion

        private static List<StaffGroup> pool = new List<StaffGroup>();

        #region Reachable Properties

        public static int Count => pool.Count;

        public static List<StaffGroup> Pool => pool;

        #endregion

        #region Public Function

        public static bool Add(Component component, string staffName, EComponentGroup groupName)
        {
            return Add(new Staff(component, staffName), groupName);
        }

        public static bool Add(Staff staff, EComponentGroup groupName)
        {
            var group = CheckGroup(groupName);

            return group == null ? false : group.Add(staff);
        }

        public static T GetStaff<T>(string staffName, EComponentGroup groupName) where T : MonoBehaviour
        {
            return GetStaff<T>(staffName, groupName, false);
        }

        public static T GetStaff<T>(string staffName, EComponentGroup groupName, bool delete) where T : MonoBehaviour
        {
            var group = GetGroup(groupName);
            var staff = group == null ? null : group[staffName].Component as T;

            if (delete) { group.Remove(staffName); }

            return staff;
        }

        public static bool Remove(string staffName, EComponentGroup groupName)
        {
            var group = GetGroup(groupName);

            return group == null ? false : group.Remove(staffName);
        }

        public static void Reset() { pool = new List<StaffGroup>(); }

        #endregion

        #region Private Function

        private static StaffGroup CheckGroup(EComponentGroup groupName)
        {
            if (GetGroup(groupName) == null) { pool.Add(new StaffGroup(groupName)); }

            return GetGroup(groupName);
        }

        public static StaffGroup GetGroup(EComponentGroup groupName)
        {
            if (pool.Count <= 0) { return null; }

            foreach (StaffGroup group in pool)
            {
                if (group.GroupName == groupName) { return group; }
            }

            return null;
        }

        #endregion
    }
}