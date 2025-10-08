using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class City
    {

        public static ML.Result GetAll() //Lista semestre
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.PruebaTecnicaContext context = new DL.PruebaTecnicaContext())
                {
                    var listCities = (from cityDB in context.Cities
                                      select cityDB).ToList();

                    if (listCities.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in listCities)
                        {
                            ML.City city = new ML.City();
                            city.IdCity = item.IdCity;
                            city.Name = item.Name;

                            result.Objects.Add(city);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registro";
                    }
                }


            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }
    }
}
