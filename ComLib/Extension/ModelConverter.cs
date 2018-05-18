using System;

namespace ComLib.Extension
{
    public static class ModelConverter
    {
        public static void Convert<F,T>(this F f, T t)
        {
            Type fType = f.GetType();
            Type tType = t.GetType();
            var listOfProperty = fType.GetProperties();
            foreach (var property in listOfProperty)
            {
                if(tType.GetProperty(property.Name)!=null)
                    if (!property.PropertyType.Name.Contains("List") && property.GetSetMethod() != null && !property.GetSetMethod().IsVirtual)
                        tType.GetProperty(property.Name).SetValue(t, property.GetValue(f, null), null);
                //TBD to add sub loop for list of sub objects.
            }
        }
    }
}
