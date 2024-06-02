using System.Collections.Generic;
using System;
using System.Reflection;
using Celeste.Components;
using UnityEditor;

namespace CelesteEditor.Components
{
    [InitializeOnLoad]
    public static class ComponentDataDrawers
    {
        private static Dictionary<Type, Type> componentDataDrawers = new Dictionary<Type, Type>();
        private static Dictionary<object, ComponentDataDrawer> instanceDataDrawers = new Dictionary<object, ComponentDataDrawer>();

        static ComponentDataDrawers()
        {
            componentDataDrawers.Clear();
            instanceDataDrawers.Clear();

            Type componentDataDrawer = typeof(ComponentDataDrawer);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (componentDataDrawer.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        CustomComponentDataDrawerAttribute customDrawerAttribute = t.GetCustomAttribute<CustomComponentDataDrawerAttribute>();

                        if (customDrawerAttribute != null)
                        {
                            UnityEngine.Debug.Log($"Found component data drawer {t.Name} for component type: {customDrawerAttribute.ComponentType.Name}");
                            componentDataDrawers.Add(customDrawerAttribute.ComponentType, t);
                        }
                    }
                }
            }
        }

        public static ComponentDataDrawer GetComponentDataDrawer(SerializedProperty dataProperty, BaseComponent component)
        {
            SerializedObject componentObject = new SerializedObject(component);

            if (instanceDataDrawers.TryGetValue(dataProperty.managedReferenceValue, out ComponentDataDrawer componentDataDrawer))
            {
                componentDataDrawer.SetData(dataProperty);
                return componentDataDrawer;
            }
            else if (componentDataDrawers.TryGetValue(component.GetType(), out Type componentDataDrawerType))
            {
                componentDataDrawer = Activator.CreateInstance(componentDataDrawerType) as ComponentDataDrawer;
                componentDataDrawer.Enable(dataProperty, componentObject);
                instanceDataDrawers.Add(dataProperty.managedReferenceValue, componentDataDrawer);
                return componentDataDrawer;
            }
            else
            {
                componentDataDrawer = new ComponentDataDrawer();
                componentDataDrawer.Enable(dataProperty, componentObject);
                instanceDataDrawers.Add(dataProperty.managedReferenceValue, componentDataDrawer);
                return componentDataDrawer;
            }
        }
    }
}
