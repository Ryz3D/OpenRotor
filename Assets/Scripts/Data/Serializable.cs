using System.Xml.Linq;

public interface Serializable {
    XElement Serialize();
    void Deserialize(XElement xml);
}
