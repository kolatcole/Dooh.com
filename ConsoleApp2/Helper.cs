using Dooh.com;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp2
{
    public static class Helper
    {

        public static List<Ad> GetDataTabletFromCSVFile(string filepath)
        {

            List<Ad> Ads = new List<Ad>();
            try
            {
                using (TextFieldParser reader = new TextFieldParser(filepath))
                {
                    reader.SetDelimiters(new string[] { "," });
                    reader.HasFieldsEnclosedInQuotes = true;

                    bool isHeader = true;

                    while (!reader.EndOfData)
                    {
                        string[] fieldData = reader.ReadFields();
                        if (!isHeader)
                        {



                            //Making empty value as null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }


                            // convert to Ad Object

                            var Ad = new Ad
                            {
                                Date = DateTime.ParseExact(fieldData[0] + " " + fieldData[1], "d/M/yyyy H:mm", CultureInfo.InvariantCulture),
                                //Date=DateTime.Now,
                                CreativeID = fieldData[2],
                                FrameID = fieldData[3],
                                DeliveredPlays = int.Parse(fieldData[4]),
                                //DeliveredPlays = 2,
                                CampaignName = fieldData[5],
                                Creative = fieldData[6],
                                DistrictName = fieldData[7],
                                PostCode = fieldData[8]
                            };







                            Ads.Add(Ad);
                        }


                        isHeader = false;


                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Ads;
        }



        public static void ExportToExcel<T>(List<T> data, string fileName, string worksheetName) where T : class
        {
            

            try
            {
                var fileInfo = new FileInfo(fileName);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(fileInfo))
                {
                    // Create the worksheet
                    var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                    // Write the headers
                    var headers = typeof(T).GetProperties().Select(p => p.Name).ToArray();
                    for (var i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }
                    worksheet.Cells[1, 2].Value = "Hour";
                    worksheet.Cells[1, 1].Value = "Date";



                    // Write the data

                    var rowData = new List<object[]>();
                    foreach (var item in data)
                    {
                        var rowValues = new List<object>();
                        var dateValue = (DateTime)typeof(T).GetProperty("Date").GetValue(item); // Replace "DateField" with the actual name of the date property
                        

                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (prop.Name == "Id")
                            {
                                rowValues.Add(dateValue.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            }
                            else if(prop.Name == "Date")
                            {
                                   rowValues.Add(dateValue.ToString("HH:mm", CultureInfo.InvariantCulture));
                            }
                            else
                                rowValues.Add(prop.GetValue(item));
                        }
                        rowData.Add(rowValues.ToArray());
                    
                    
                    }



                    worksheet.Cells["A2"].LoadFromArrays(rowData);

                    // Auto-fit the columns
                    worksheet.Cells.AutoFitColumns();

                    package.Save();

                    package.Dispose();
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
            
        }

        public static string GetRootDirectory()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));

        }


        public static void CreateGraph(List<Ad> Ads, string graphPath)
        {


            try
            {

                var plotModel = new PlotModel();

                var barItems = new List<BarItem>();
                var catItems = new List<string>();

                // Create the category axis (Y-axis)
                var categoryAxisY = new CategoryAxis()
                {
                    Position = AxisPosition.Left,
                    Key = "DistrictName",
                    Minimum = 0,
                    Maximum = 100,
                    Title = "DistrictName",
                    LabelField = "DistrictName" 
                };

                // Create the bar series
                var barSeries = new BarSeries
                {
                    ItemsSource = barItems,
                    LabelPlacement = LabelPlacement.Inside,
                    LabelFormatString = "{0:N0}",
                    BarWidth = 0.5
                };

                // Create the category axis (X-axis)
                var categoryAxis = new CategoryAxis()
                {
                    Position = AxisPosition.Bottom,
                    ItemsSource = catItems,
                    AbsoluteMinimum = 0, // Set minimum to -0.5 to give a 10% padding
                    MaximumMargin = 100
                };


                foreach (var ad in Ads)
                {
                    catItems.Add(ad.DistrictName);
                    barItems.Add(new BarItem { Value = int.Parse(ad.FrameID.ToString().Substring(8)) });
                    categoryAxisY.Labels.Add(ad.DistrictName);

                }



                // Add the bar series to the plot model
                plotModel.Series.Add(barSeries);

                plotModel.Axes.Add(categoryAxis);
                plotModel.Axes.Add(categoryAxisY);

               

                
                // Create a new FileStream with the path and file name
         
                using (var stream = new FileStream(graphPath, FileMode.Create))
                {
                    // Create a new PDF exporter
                    var pdfExporter = new PdfExporter { Width = 2000, Height = 1000 };

                    // Export the plot model to the PDF stream
                    pdfExporter.Export(plotModel, stream);

                   
                }
            }
            catch(Exception ex)
            {

            }
           


        }






    }
}
