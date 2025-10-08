using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Education
    {

        public static ML.Result GetAll() //Lista semestre
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.PruebaTecnicaContext context = new DL.PruebaTecnicaContext())
                {

                    var listEducation = (from eduDB in context.Educations
                                         select eduDB).ToList();

                    if (listEducation.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in listEducation)
                        {
                            ML.Education education = new ML.Education();
                            education.IdEducation = item.IdEducation;
                            education.Name = item.Name;

                            result.Objects.Add(education);
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

