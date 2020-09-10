using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vault
{
    public class ElementsManager
    {
        private readonly List<Element> elements;
        private static ElementsManager instance = null;
        int id = 1;

        public static ElementsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ElementsManager();
                }

                return instance;
            }
        }

        private ElementsManager()
        {
            elements = new List<Element>();
        }

        public void AddElement(Element element)
        {
            element.ID = id;
            id++;
            elements.Add(element);
        }

        public void RemoveElement(Element element)
        {
            elements.Remove(element);
        }
    }
}
