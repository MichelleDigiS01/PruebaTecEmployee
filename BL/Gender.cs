using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Gender
    {
        public static ML.Result GetAll() 
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.PruebaTecnicaContext context = new DL.PruebaTecnicaContext())
                {
                    var listGender = (from genderDB in context.Genders
                                      select genderDB).ToList();

                    if (listGender.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in listGender)
                        {
                            ML.Gender gender = new ML.Gender();
                            gender.IdGender= item.IdGender;
                            gender.Name = item.Name;

                            result.Objects.Add(gender);
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
