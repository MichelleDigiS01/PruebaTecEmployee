using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Employee
    {
        private readonly DL.PruebaTecnicaContext _context;

        public Employee(DL.PruebaTecnicaContext context)
        {
            _context = context;
        }


        public ML.Result GetAll(ML.Employee employee)
        {
            ML.Result result = new ML.Result();

            try
            {
                // Define un DTO temporal para mapear los resultados del SP
                var resultQuery = _context.DTOs.FromSqlInterpolated($"EXEC EmployeeGetAll {employee.JoiningYear},{employee.PaymentTier},{employee.Age},{employee.EverBenched},{employee.Experience},{employee.LeaveOrNot},{employee.City.IdCity},{employee.Education.IdEducation},{employee.Gender.IdGender}").ToList();

                if (resultQuery.Count > 0)
                {
                    result.Objects = new List<object>();
                    foreach (var employeObj in resultQuery)
                    {

                        ML.Employee employees = new ML.Employee();

                        employees.IdEmployee = employeObj.IdEmployee;
                        employees.JoiningYear = employeObj.JoiningYear;
                        employees.PaymentTier = employeObj.PaymentTier;
                        employees.Age = employeObj.Age;
                        employees.EverBenched = employeObj.EverBenched;
                        employees.Experience = employeObj.Experience;
                        employees.LeaveOrNot = employeObj.LeaveOrNot;

                        employees.City = new ML.City();
                        employees.City.IdCity = employeObj.IdCity;
                        employees.City.Name = employeObj.CityName;

                        employees.Education = new ML.Education();

                        employees.Education.IdEducation = employeObj.IdEducation;
                        employees.Education.Name = employeObj.EducationName;

                        employees.Gender = new ML.Gender();

                        employees.Gender.IdGender = employeObj.IdGender;
                        employees.Gender.Name = employeObj.GenderName;

                        result.Objects.Add(employees);

                    }
                    result.Correct = true;

                }
                else
                {
                    result.Correct = false;
                }



            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result Add(ML.Employee employee)
        {
            ML.Result result = new ML.Result();

            try
            {
                var query = _context.Database.ExecuteSqlInterpolated($@"EXEC EmployeeAdd {employee.JoiningYear},{employee.PaymentTier},{employee.Age},{employee.EverBenched},{employee.Experience},{employee.LeaveOrNot},{employee.City.IdCity},{employee.Gender.IdGender}, {employee.Education.IdEducation}");

                if (query > 0)
                {
                    result.Correct = true;

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result GetById(int idEmployee)
        {
            ML.Result result = new ML.Result();

            try
            {
                var ResultQuery = (from employee in _context.Employees
                                   join cityDb in _context.Cities on employee.IdCity equals cityDb.IdCity
                                   join eduDb in _context.Educations on employee.IdEducation equals eduDb.IdEducation
                                   join genderDb in _context.Genders on employee.IdGender equals genderDb.IdGender
                                   where employee.IdEmployee == idEmployee
                                   select employee)

                    .FirstOrDefault();

                if (ResultQuery != null)
                {
                    result.Object = new List<object>();

                    ML.Employee employee = new ML.Employee();

                    employee.IdEmployee = ResultQuery.IdEmployee;

                    employee.JoiningYear = ResultQuery.JoiningYear;
                    employee.PaymentTier = ResultQuery.PaymentTier;
                    employee.Age = ResultQuery.Age;
                    employee.EverBenched = ResultQuery.EverBenched;
                    employee.Experience = ResultQuery.Experience;
                    employee.LeaveOrNot = ResultQuery.LeaveOrNot;

                    employee.City = new ML.City();
                    employee.City.IdCity = ResultQuery.IdCity;

                    employee.Education = new ML.Education();

                    employee.Education.IdEducation = ResultQuery.IdEducation;

                    employee.Gender = new ML.Gender();

                    employee.Gender.IdGender = ResultQuery.IdGender;



                    result.Object = employee;

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No encontrado";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result Update(ML.Employee employee)
        {
            ML.Result result = new ML.Result();

            try
            {
                var ResultQuery = (from employeDb in _context.Employees
                                        where employeDb.IdEmployee == employee.IdEmployee
                                        select employeDb).SingleOrDefault();


                if (ResultQuery != null)
                {
                    ResultQuery.JoiningYear = employee.JoiningYear;
                    ResultQuery.PaymentTier = employee.PaymentTier;
                    ResultQuery.Age = employee.Age;
                    ResultQuery.EverBenched = employee.EverBenched;
                    ResultQuery.Experience = employee.Experience;
                    ResultQuery.LeaveOrNot = employee.LeaveOrNot;
                    ResultQuery.IdCity = employee.City.IdCity;
                    ResultQuery.IdGender = employee.Gender.IdGender;
                    ResultQuery.IdEducation = employee.Education.IdEducation;

                    _context.SaveChanges();
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Employee not found";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result Delete(int idEmployee)
        {
            ML.Result result = new ML.Result();

            try
            {
                
                var resultQuery = (from employeeDb in _context.Employees
                                   where employeeDb.IdEmployee == idEmployee
                                   select employeeDb).First();

                _context.Employees.Remove(resultQuery);

                int RowsAffected = _context.SaveChanges();

                if (RowsAffected > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result GetByBenched(string Benched)
        {
            ML.Result result = new ML.Result();

            try
            {
                var ResultQuery = _context.EmployeesBenchedDTO.FromSqlInterpolated($"EXEC EmployeesBenched {Benched}").ToList();

                if (ResultQuery.Count > 0)
                {
                    result.Objects = new List<object>();
                    foreach (var item in ResultQuery)
                    {
                        ML.Employee employee = new ML.Employee();

                        employee.IdEmployee = item.IdEmployee;
                        employee.JoiningYear = item.JoiningYear;

                        employee.City = new ML.City();
                        employee.City.Name = item.CityName;

                        employee.Education = new ML.Education();
                        employee.Education.Name = item.EducationName;

                        employee.Gender = new ML.Gender();
                        employee.Gender.Name = item.GenderName;

                        result.Objects.Add(employee);

                    }

                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontraron datos";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        

    }
}
