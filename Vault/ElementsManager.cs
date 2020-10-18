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
        private int id = 0;

        public int Count => elements.Count;
        public Element this[int index] { get => elements[index]; set => elements[index] = value; }

        public static ElementsManager Instance
        {
            get
            {
                if (instance == null) instance = new ElementsManager();
                return instance;
            }
        }


        private ElementsManager()
        {
            elements = new List<Element>();
        }

        public Element SaveElement(Element element) => element.ID == -1 ? AddElement(element) : EditElement(element);

        public bool RemoveElement(Element element)
        {
            if (ElementExists(element))
            {
                elements.Remove(element);
                return true;
            }
            else return false;
        }

        public Element GetElementByID(int id)
        {
            int index = elements.IndexOf(new Element() { ID = id });
            if (index != -1) return elements[index];
            else return null;
        }

        public bool ElementExists(Element element) => elements.Contains(element);

        private Element AddElement(Element element)
        {
            element.ID = id;
            id++;
            elements.Add(element);
            return element;
        }

        private Element EditElement(Element element)
        {
            int index = elements.IndexOf(element);
            if (index != -1)
            {
                elements[index] = element;
                return element;
            }
            else return null;
        }
    }
}
