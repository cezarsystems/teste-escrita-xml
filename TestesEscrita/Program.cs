using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TestesEscrita
{
    class Program
    {
        static void Main(string[] args)
        {
            GerenciarControleAlertaXML(1109, 3, DateTime.Now);
        }

        static bool GerenciarControleAlertaXML(long codigoBoleto, short statusBoleto, DateTime dataStatusBoleto)
        {
            XDocument xDocument;

            string nomeArquivoControleXML = "ctrl_boleto_sem_retorno.xml";

            if (File.Exists(nomeArquivoControleXML))
            {
                xDocument = XDocument.Load(nomeArquivoControleXML);

                var boletosResult = from query in xDocument.Descendants("boleto")
                               where query.Attribute("codigo").Value == codigoBoleto.ToString()
                               select new
                               {
                                   CodigoBoleto = query.Attribute("codigo").Value,
                                   CodigoStatusBoleto = query.Element("status").Value,
                                   DataStatusBoleto = query.Element("data-status").Value
                               };

                RemoveNoXML(nomeArquivoControleXML, codigoBoleto);

                

                if (boletosResult.Count() == 1)
                {
                    // faz a comparação                    
                }
                else if (boletosResult.Count() < 1)
                {
                    // Salva os dados do boleto no xml                    
                }
            }
            else
            {
                
            }

            return true;
        }

        void NovoXML(string nomeArquivoControleXML, long codigoBoleto, short statusBoleto, DateTime dataStatusBoleto)
        {
            // Novo arquivo
            XDocument xDocument = new XDocument(
                new XDeclaration("1.0", "uft-8", null),
                new XElement("boletos",
                    new XElement("boleto", new XAttribute("codigo", codigoBoleto),
                        new XElement("status", statusBoleto),
                        new XElement("data-status", dataStatusBoleto))));

            xDocument.Save(nomeArquivoControleXML);
        }

        static void NovoNoXML(string nomeArquivoControleXML, long codigoBoleto, short statusBoleto, DateTime dataStatusBoleto)
        {
            // Carrega o arquivo XML
            XDocument xDocument = XDocument.Load(nomeArquivoControleXML);

            // Adicionando novo nó
            xDocument.Element("boletos").Add(
                    new XElement("boleto", new XAttribute("codigo", 2424),
                        new XElement("status", 9090),
                        new XElement("data-status", DateTime.Now.AddDays(200))));

            xDocument.Save(nomeArquivoControleXML);
        }

        static void RemoveNoXML(string nomeArquivoControleXML, long codigoBoleto)
        {
            // Carrega o arquivo XML
            XDocument xDocument = XDocument.Load(nomeArquivoControleXML);

            // Removendo Nó baseado no código do boleto
            xDocument.Root.Elements().Where(b => b.Attribute("codigo").Value == codigoBoleto.ToString()).Remove();

            xDocument.Save(nomeArquivoControleXML);
        }
    }            
}