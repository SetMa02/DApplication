using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

namespace DefaultNamespace.Admin
{
    public class EmployeeContentUpdater : MonoBehaviour
    {
        public List<AdminElement> Employees = new List<AdminElement>() { };
        [SerializeField] private GameObject _container;
        [SerializeField] private AdminElement _prefab;
        [SerializeField] private FireBase _fireBase;
        
        private int _employeeCount;
        
        public void StartLoadEmployee()
        {
            StartCoroutine(LoadEmployees());
        }
        
        private IEnumerator LoadEmployees()
        {
            ClearChildren();
            var dbTask = _fireBase.DBreference.GetValueAsync();
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else
            {
                DataSnapshot snapshot = dbTask.Result;
                _employeeCount = Convert.ToInt32(snapshot.Child("Admins").ChildrenCount);

                for (int i = 0; i < _employeeCount; i++)
                {
                    AdminElement adminElement = Instantiate(_prefab, _container.transform);
                   
                    int currentAdmin = i + 1;
                    adminElement.Id = currentAdmin;
                    adminElement.Login = snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("Login").Value.ToString();
                    adminElement.Password = snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("Password").Value.ToString();
                    
                    adminElement.ElementCreate();
                    
                    if (snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("CanAdd").Value.Equals(true))
                    {
                        adminElement.canAdd = true;
                    }
                    if (snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("CanDelete").Value.Equals(true))
                    {
                        adminElement.canDelete = true;
                    }
                    if (snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("CanChange").Value.Equals(true))
                    {
                        adminElement.canChange = true;
                    }
                    if (snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("CanAddEmployee").Value.Equals(true))
                    {
                        adminElement.CanAddEmp = true;
                    }
                    if (snapshot.Child("Admins").Child(currentAdmin.ToString()).Child("CanDeleteEmployee").Value.Equals(true))
                    {
                        adminElement.canDeleteEmp = true;
                    }
                    Employees.Add(adminElement);
                  
                }
            }
        }
        
      
        
        private void ClearChildren()
        {
            int i = 0;

            GameObject[] allChildren = new GameObject[_container.transform.childCount];

            foreach (Transform child in _container.transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }
        
            foreach (GameObject child in allChildren)
            {
                DestroyImmediate(child.gameObject);
            }
        }

    }
}