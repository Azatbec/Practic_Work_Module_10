using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practic_Work_Module_10_2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace CorporateHierarchy
    {
        // Абстрактный класс компонента
        public abstract class OrganizationComponent
        {
            public string Name { get; }
            public OrganizationComponent(string name) => Name = name;

            public virtual void Add(OrganizationComponent component) { }
            public virtual void Remove(OrganizationComponent component) { }
            public virtual void DisplayHierarchy(int depth = 0) { }
            public virtual decimal CalculateBudget() => 0;
            public virtual int CountEmployees() => 0;
            public virtual OrganizationComponent FindEmployee(string name) => null;
        }

        // Класс для сотрудников
        public class Employee : OrganizationComponent
        {
            public string Position { get; }
            public decimal Salary { get; set; }

            public Employee(string name, string position, decimal salary) : base(name)
            {
                Position = position;
                Salary = salary;
            }

            public override void DisplayHierarchy(int depth = 0)
            {
                Console.WriteLine($"{new string(' ', depth * 2)}- {Name} ({Position}), Salary: {Salary:C}");
            }

            public override decimal CalculateBudget() => Salary;

            public override int CountEmployees() => 1;

            public override OrganizationComponent FindEmployee(string name) => Name.Equals(name, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        // Класс для контракторов
        public class Contractor : OrganizationComponent
        {
            public string Position { get; }
            public decimal FixedPay { get; }

            public Contractor(string name, string position, decimal fixedPay) : base(name)
            {
                Position = position;
                FixedPay = fixedPay;
            }

            public override void DisplayHierarchy(int depth = 0)
            {
                Console.WriteLine($"{new string(' ', depth * 2)}- {Name} ({Position}), Fixed Pay: {FixedPay:C} [Contractor]");
            }

            public override int CountEmployees() => 1;

            public override OrganizationComponent FindEmployee(string name) => Name.Equals(name, StringComparison.OrdinalIgnoreCase) ? this : null;
        }

        // Класс для отделов
        public class Department : OrganizationComponent
        {
            private readonly List<OrganizationComponent> _components = new();

            public Department(string name) : base(name) { }

            public override void Add(OrganizationComponent component)
            {
                _components.Add(component);
            }

            public override void Remove(OrganizationComponent component)
            {
                _components.Remove(component);
            }

            public override void DisplayHierarchy(int depth = 0)
            {
                Console.WriteLine($"{new string(' ', depth * 2)}* Department: {Name}");
                foreach (var component in _components)
                {
                    component.DisplayHierarchy(depth + 1);
                }
            }

            public override decimal CalculateBudget()
            {
                return _components.OfType<Employee>().Sum(e => e.CalculateBudget()) +
                       _components.OfType<Department>().Sum(d => d.CalculateBudget());
            }

            public override int CountEmployees()
            {
                return _components.Sum(component => component.CountEmployees());
            }

            public override OrganizationComponent FindEmployee(string name)
            {
                foreach (var component in _components)
                {
                    var found = component.FindEmployee(name);
                    if (found != null) return found;
                }
                return null;
            }

            public List<OrganizationComponent> GetAllEmployees()
            {
                var employees = new List<OrganizationComponent>();
                foreach (var component in _components)
                {
                    if (component is Employee || component is Contractor)
                    {
                        employees.Add(component);
                    }
                    else if (component is Department department)
                    {
                        employees.AddRange(department.GetAllEmployees());
                    }
                }
                return employees;
            }
        }

        // Точка входа
        public class Program
        {
            public static void Main()
            {
                // Создание сотрудников
                Employee emp1 = new Employee("Alice", "Developer", 60000);
                Employee emp2 = new Employee("Bob", "Designer", 55000);
                Contractor contractor1 = new Contractor("Charlie", "Freelance Developer", 30000);

                // Создание отдела IT и добавление сотрудников
                Department itDepartment = new Department("IT Department");
                itDepartment.Add(emp1);
                itDepartment.Add(emp2);
                itDepartment.Add(contractor1);

                // Создание отдела HR и сотрудников
                Employee emp3 = new Employee("Diana", "HR Manager", 70000);
                Department hrDepartment = new Department("HR Department");
                hrDepartment.Add(emp3);

                // Создание главного департамента и добавление подчиненных отделов
                Department mainDepartment = new Department("Corporate HQ");
                mainDepartment.Add(itDepartment);
                mainDepartment.Add(hrDepartment);

                // Отображение структуры
                Console.WriteLine("--- Organization Hierarchy ---");
                mainDepartment.DisplayHierarchy();

                // Вычисление общего бюджета
                Console.WriteLine($"\nTotal Budget of Corporate HQ: {mainDepartment.CalculateBudget():C}");

                // Подсчет количества сотрудников
                Console.WriteLine($"\nTotal Employees in Corporate HQ: {mainDepartment.CountEmployees()}");

                // Поиск сотрудника
                string searchName = "Alice";
                OrganizationComponent foundEmployee = mainDepartment.FindEmployee(searchName);
                if (foundEmployee != null)
                {
                    Console.WriteLine($"\nEmployee {searchName} found:");
                    foundEmployee.DisplayHierarchy();
                }
                else
                {
                    Console.WriteLine($"\nEmployee {searchName} not found.");
                }

                // Отображение всех сотрудников отдела IT
                Console.WriteLine("\nAll employees in IT Department:");
                foreach (var employee in itDepartment.GetAllEmployees())
                {
                    employee.DisplayHierarchy();
                }
            }
        }
    }

}
