using System.Collections.Generic;
using System.Configuration;

namespace NoSqlExplorer.DockerAdapter.Configuration
{
  [ConfigurationCollection(typeof(DockerContainerConfigCollection), AddItemName = "dockerContainer", CollectionType = ConfigurationElementCollectionType.BasicMap)]
  public class DockerContainerConfigCollection : ConfigurationElementCollection, IEnumerable<DockerContainerConfigElement>
  {
    public DockerContainerConfigElement this[int index]
    {
      get { return (DockerContainerConfigElement) BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public void Add(DockerContainerConfigElement element)
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

    public void Remove(DockerContainerConfigElement element)
    {
      BaseRemove(GetElementKey(element));
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return new DockerContainerConfigElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      var dockerContainerConfigElement = (DockerContainerConfigElement) element;
      return $"{dockerContainerConfigElement.Name}:{dockerContainerConfigElement.Port}";
    }

    public new IEnumerator<DockerContainerConfigElement> GetEnumerator()
    {
      for (var i = 0; i < Count; ++i)
      {
        yield return BaseGet(i) as DockerContainerConfigElement;
      }
    }
  }
}
