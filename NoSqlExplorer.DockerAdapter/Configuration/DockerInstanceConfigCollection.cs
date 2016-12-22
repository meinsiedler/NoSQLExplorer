using System.Collections.Generic;
using System.Configuration;

namespace NoSqlExplorer.DockerAdapter.Configuration
{
  [ConfigurationCollection(typeof(DockerInstanceConfigElement), AddItemName = "dockerInstance", CollectionType = ConfigurationElementCollectionType.BasicMap)]
  public class DockerInstanceConfigCollection : ConfigurationElementCollection, IEnumerable<DockerInstanceConfigElement>
  {
    public DockerInstanceConfigElement this[int index]
    {
      get { return (DockerInstanceConfigElement) BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public void Add(DockerInstanceConfigElement element)
    {
      BaseAdd(element);
    }

    public void Clear()
    {
      BaseClear();
    }

    public void RemoveAt(int index)
    {
      BaseRemoveAt(index);
    }

    public void Remove(DockerInstanceConfigElement element)
    {
      BaseRemove(GetElementKey(element));
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return new DockerInstanceConfigElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      var dockerInstanceConfigElement = (DockerInstanceConfigElement) element;
      return $"{dockerInstanceConfigElement.Host}:{dockerInstanceConfigElement.Port}";
    }

    public new IEnumerator<DockerInstanceConfigElement> GetEnumerator()
    {
      for (var i = 0; i < Count; ++i)
      {
        yield return BaseGet(i) as DockerInstanceConfigElement;
      }
    }
  }
}
