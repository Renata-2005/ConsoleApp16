

using System;
using System.IO;
using System.Reflection;
using System.Text;

abstract class Restarunt
{
    // count of tablets
    public int TabletCount = 20;

    protected string Name; 
    protected string Adres; 

    public int Table
    {
        get { return TabletCount; }
        set { TabletCount = value; }
    }

    public abstract int TabletId();

    protected Restarunt(string name, string adres)
    {
        Name = name;
        Adres = adres;
    }

    
    public string GetName()
    {
        return Name;
    }

    
    public string GetAdres()
    {
        return Adres;
    }
}

class RestaurantTable : Restarunt
{
    public int TabletNumber;
    private string NameVisitor;

    private bool[] tables;

    public string Name
    {
        get; set;
    }

    public int Number
    {
        get
        {
            return TabletNumber;
        }
        set
        {
            if (value > 20 || value < 0)
            {
                Console.WriteLine("Введите корректный номер стола.");
            }
            else { TabletNumber = value; }
        }
    }

    public RestaurantTable(string name, string adres) : base(name, adres)
    {
        tables = new bool[TabletCount];
        for (int i = 0; i < TabletCount; i++)
        {
            tables[i] = false;
        }
    }

    private void CheckTable()
    {
        Console.WriteLine("Свободные столы:");
        for (int i = 0; i < tables.Length; i++)
        {
            if (!tables[i])
            {
                Console.WriteLine($"Стол {i + 1} свободен.");
            }
        }
    }

    public void ShowAvailableTables()
    {
        CheckTable();
    }

    public void ReserveTable(int tableNumber)
    {
        if (tableNumber < 1 || tableNumber > TabletCount)
        {
            Console.WriteLine("Неверный номер стола.");
            return;
        }

        if (tables[tableNumber - 1])
        {
            Console.WriteLine($"Стол {tableNumber} уже занят.");
        }
        else
        {
            tables[tableNumber - 1] = true;
            Console.WriteLine($"Стол {tableNumber} успешно забронирован.");
        }
    }

    public override int TabletId()
    {
        
        return TabletNumber;
    }

    
    public static int GetTotalTables()
    {
        return 20; 
    }
}

class Program
{
    static void Main(string[] args)
    {
        RestaurantTable restaurant = new RestaurantTable("Ресторан", "ул. Пушкина, д. 10");

        restaurant.ShowAvailableTables();

        restaurant.ReserveTable(5);
        restaurant.ReserveTable(10);

        restaurant.ShowAvailableTables();

        
        Console.WriteLine($"Общее количество столов: {RestaurantTable.GetTotalTables()}");





        Type restaurantTableType = typeof(RestaurantTable);

   
        Console.WriteLine("Информация о всех членах класса RestaurantTable:");

        
        Console.WriteLine("\nМетоды:");
        MethodInfo[] methods = restaurantTableType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            Console.WriteLine($"- {method.ReturnType} {method.Name}()");
        }

        
        Console.WriteLine("\nСвойства:");
        PropertyInfo[] properties = restaurantTableType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
        foreach (var property in properties)
        {
            Console.WriteLine($"- {property.PropertyType} {property.Name}");
        }

        
        Console.WriteLine("\nПоля:");
        FieldInfo[] fields = restaurantTableType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            Console.WriteLine($"- {field.FieldType} {field.Name} ({(field.IsPrivate ? "private" : "public")})");
        }

        
        Console.WriteLine("\nКонструкторы:");
        ConstructorInfo[] constructors = restaurantTableType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var constructor in constructors)
        {
            Console.WriteLine($"- {constructor.Name}");
        }

        GenerateHtml(typeof(Restarunt), "Table.html");
        GenerateHtml(typeof(RestaurantTable), "RestaurantTable.html");

        
        GetConstructorsAndCreateObjects(restaurantTableType);

        
        GetMethodsAndInvokePrivateMethod(restaurantTableType);
    }

    private static void GenerateHtml(Type type, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("<html>");
            writer.WriteLine("<head><title>Documentation for " + type.Name + "</title></head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<h1>" + type.Name + "</h1>");
            writer.WriteLine("<hr>");

           
            writer.WriteLine("<h2>Public Members</h2>");
            writer.WriteLine("<h3>Fields</h3>");
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                writer.WriteLine("<p>" + field.FieldType + " " + field.Name + "</p>");
            }

            writer.WriteLine("<h3>Properties</h3>");
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                writer.WriteLine("<p>" + property.PropertyType + " " + property.Name + "</p>");
            }

            writer.WriteLine("<h3>Methods</h3>");
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (!method.IsSpecialName)
                {
                    writer.WriteLine("<p>" + method.ReturnType + " " + method.Name + "</p>");
                }
            }

            
            writer.WriteLine("<h2>Non-Public Members</h2>");
            writer.WriteLine("<h3>Fields</h3>");
            foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                writer.WriteLine("<p>" + field.FieldType + " " + field.Name + "</p>");
            }

            writer.WriteLine("<h3>Properties</h3>");
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                writer.WriteLine("<p>" + property.PropertyType + " " + property.Name + "</p>");
            }

            writer.WriteLine("<h3>Methods</h3>");
            foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                if (!method.IsSpecialName)
                {
                    writer.WriteLine("<p>" + method.ReturnType + " " + method.Name + "</p>");
                }
            }

            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }
    }

    
    private static void GetConstructorsAndCreateObjects(Type type)
    {
        Console.WriteLine("\nПолучение списка конструкторов и создание объектов:");

        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        foreach (var constructor in constructors)
        {
            Console.WriteLine($"Конструктор: {constructor.Name}");
            Console.WriteLine($"Модификатор доступа: {(constructor.IsPublic ? "Public" : "Non-Public")}");

            
            ParameterInfo[] parameters = constructor.GetParameters();
            Console.WriteLine("Параметры:");
            foreach (var parameter in parameters)
            {
                Console.WriteLine($"- {parameter.ParameterType} {parameter.Name}");
            }

            
            if (parameters.Length == 2 && parameters[0].ParameterType == typeof(string) && parameters[1].ParameterType == typeof(string))
            {
                object[] constructorArgs = new object[] { "Новый ресторан", "ул. Лермонтова, д. 5" };
                object instance = constructor.Invoke(constructorArgs);
                Console.WriteLine("Объект успешно создан!");
            }
        }
    }

    
    private static void GetMethodsAndInvokePrivateMethod(Type type)
    {
        Console.WriteLine("\nПолучение всех методов и вызов приватного метода:");

        
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            Console.WriteLine($"Метод: {method.Name}, Тип: {(method.IsPublic ? "Public" : "Non-Public")}");
        }

       
        MethodInfo checkTableMethod = type.GetMethod("CheckTable", BindingFlags.NonPublic | BindingFlags.Instance);
        if (checkTableMethod != null)
        {
            
            object instance = Activator.CreateInstance(type, "Ресторан", "ул. Пушкина, д. 10");

            
            checkTableMethod.Invoke(instance, null);
            Console.WriteLine("Приватный метод CheckTable успешно вызван!");
        }
        else
        {
            Console.WriteLine("Метод CheckTable не найден.");
        }
    }
}