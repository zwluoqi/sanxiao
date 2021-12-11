#if !UNITY_WINRT || UNITY_EDITOR || UNITY_WP8
#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#if !(JSONNET_XMLDISABLE || UNITY_WP8 || UNITY_WINRT)
using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json.Utilities;
using System.Linq;

namespace Newtonsoft.Json.Converters
{
  #region XmlNodeWrappers

  internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument
  {
    private XmlDocument _document;

    public XmlDocumentWrapper(XmlDocument document)
      : base(document)
    {
      _document = document;
    }

    public IXmlNode CreateComment(string data)
    {
      return new XmlNodeWrapper(_document.CreateComment(data));
    }

    public IXmlNode CreateTextNode(string text)
    {
      return new XmlNodeWrapper(_document.CreateTextNode(text));
    }

    public IXmlNode CreateCDataSection(string data)
    {
      return new XmlNodeWrapper(_document.CreateCDataSection(data));
    }

    public IXmlNode CreateWhitespace(string text)
    {
      return new XmlNodeWrapper(_document.CreateWhitespace(text));
    }

    public IXmlNode CreateSignificantWhitespace(string text)
    {
      return new XmlNodeWrapper(_document.CreateSignificantWhitespace(text));
    }

    public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
    {
      return new XmlNodeWrapper(_document.CreateXmlDeclaration(version, encoding, standalone));
    }

    public IXmlNode CreateProcessingInstruction(string target, string data)
    {
      return new XmlNodeWrapper(_document.CreateProcessingInstruction(target, data));
    }

    public IXmlElement CreateElement(string elementName)
    {
      return new XmlElementWrapper(_document.CreateElement(elementName));
    }

    public IXmlElement CreateElement(string qualifiedName, string namespaceURI)
    {
      return new XmlElementWrapper(_document.CreateElement(qualifiedName, namespaceURI));
    }

    public IXmlNode CreateAttribute(string name, string value)
    {
      XmlNodeWrapper attribute = new XmlNodeWrapper(_document.CreateAttribute(name));
      attribute.Value = value;

      return attribute;
    }

    public IXmlNode CreateAttribute(string qualifiedName, string namespaceURI, string value)
    {
      XmlNodeWrapper attribute = new XmlNodeWrapper(_document.CreateAttribute(qualifiedName, namespaceURI));
      attribute.Value = value;

      return attribute;
    }

    public IXmlElement DocumentElement
    {
      get
      {
        if (_document.DocumentElement == null)
          return null;

        return new XmlElementWrapper(_document.DocumentElement);
      }
    }
  }

  internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement
  {
    private XmlElement _element;

    public XmlElementWrapper(XmlElement element)
      : base(element)
    {
      _element = element;
    }

    public void SetAttributeNode(IXmlNode attribute)
    {
      XmlNodeWrapper xmlAttributeWrapper = (XmlNodeWrapper)attribute;

      _element.SetAttributeNode((XmlAttribute) xmlAttributeWrapper.WrappedNode);
    }

    public string GetPrefixOfNamespace(string namespaceURI)
    {
      return _element.GetPrefixOfNamespace(namespaceURI);
    }
  }

  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration
  {
    private XmlDeclaration _declaration;

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base(declaration)
    {
      _declaration = declaration;
    }

    public string Version
    {
      get { return _declaration.Version; }
    }

    public string Encoding
    {
      get { return _declaration.Encoding; }
      set { _declaration.Encoding = value; }
    }

    public string Standalone
    {
      get { return _declaration.Standalone; }
      set { _declaration.Standalone = value; }
    }
  }

  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;

    public XmlNodeWrapper(XmlNode node)
    {
      _node = node;
    }

    public object WrappedNode
    {
      get { return _node; }
    }

    public XmlNodeType NodeType
    {
      get { return _node.NodeType; }
    }

    public string Name
    {
      get { return _node.Name; }
    }

    public string LocalName
    {
      get { return _node.LocalName; }
    }

    public IList<IXmlNode> ChildNodes
    {
      get { return _node.ChildNodes.Cast<XmlNode>().Select(n => WrapNode(n)).ToList(); }
    }

    private IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.XmlDeclaration:
          return new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return new XmlNodeWrapper(node);
      }
    }

    public IList<IXmlNode> Attributes
    {
      get
      {
        if (_node.Attributes == null)
          return null;

        return _node.Attributes.Cast<XmlAttribute>().Select(a => WrapNode(a)).ToList();
      }
    }

    public IXmlNode ParentNode
    {
      get
      {
        XmlNode node = (_node is XmlAttribute)
                         ? ((XmlAttribute) _node).OwnerElement
                         : _node.ParentNode;
        
        if (node == null)
          return null;

        return WrapNode(node);
      }
    }

    public string Value
    {
      get { return _node.Value; }
      set { _node.Value = value; }
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper) newChild;
      _node.AppendChild(xmlNodeWrapper._node);

      return newChild;
    }

    public string Prefix
    {
      get { return _node.Prefix; }
    }

    public string NamespaceURI
    {
      get { return _node.NamespaceURI; }
    }
  }

  #endregion

  #region Interfaces
  internal interface IXmlDocument : IXmlNode
  {
    IXmlNode CreateComment(string text);
    IXmlNode CreateTextNode(string text);
    IXmlNode CreateCDataSection(string data);
    IXmlNode CreateWhitespace(string text);
    IXmlNode CreateSignificantWhitespace(string text);
    IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);
    IXmlNode CreateProcessingInstruction(string target, string data);
    IXmlElement CreateElement(string elementName);
    IXmlElement CreateElement(string qualifiedName, string namespaceURI);
    IXmlNode CreateAttribute(string name, string value);
    IXmlNode CreateAttribute(string qualifiedName, string namespaceURI, string value);

    IXmlElement DocumentElement { get; }
  }

  internal interface IXmlDeclaration : IXmlNode
  {
    string Version { get; }
    string Encoding { get; set; }
    string Standalone { get; set; }
  }

  internal interface IXmlElement : IXmlNode
  {
    void SetAttributeNode(IXmlNode attribute);
    string GetPrefixOfNamespace(string namespaceURI);
  }

  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }
    string LocalName { get; }
    IList<IXmlNode> ChildNodes { get; }
    IList<IXmlNode> Attributes { get; }
    IXmlNode ParentNode { get; }
    string Value { get; set; }
    IXmlNode AppendChild(IXmlNode newChild);
    string NamespaceURI { get; }
    object WrappedNode { get; }
  }
  #endregion

  /// <summary>
  /// Converts XML to and from JSON.
  /// </summary>
  public class XmlNodeConverter : JsonConverter
  {
    private const string TextName = "#text";
    private const string CommentName = "#comment";
    private const string CDataName = "#cdata-section";
    private const string WhitespaceName = "#whitespace";
    private const string SignificantWhitespaceName = "#significant-whitespace";
    private const string DeclarationName = "?xml";
    private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

    /// <summary>
    /// Gets or sets the name of the root element to insert when deserializing to XML if the JSON structure has produces multiple root elements.
    /// </summary>
    /// <value>The name of the deserialize root element.</value>
    public string DeserializeRootElementName { get; set; }

    /// <summary>
    /// Gets or sets a flag to indicate whether to write the Json.NET array attribute.
    /// This attribute helps preserve arrays when converting the written XML back to JSON.
    /// </summary>
    /// <value><c>true</c> if the array attibute is written to the XML; otherwise, <c>false</c>.</value>
    public bool WriteArrayAttribute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write the root JSON object.
    /// </summary>
    /// <value><c>true</c> if the JSON root object is omitted; otherwise, <c>false</c>.</value>
    public bool OmitRootObject { get; set; }

    #region Writing
    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <param name="value">The value.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      IXmlNode node = WrapXml(value);

      XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
      PushParentNamespaces(node, manager);

      if (!OmitRootObject)
        writer.WriteStartObject();

      SerializeNode(writer, node, manager, !OmitRootObject);
      
      if (!OmitRootObject)
        writer.WriteEndObject();
    }

    private IXmlNode WrapXml(object value)
    {
#if !(UNITY_ANDROID || (UNITY_IOS || UNITY_IPHONE))
      if (value is XmlNode)
        return new XmlNodeWrapper((XmlNode)value);
#endif
      
      throw new ArgumentException("Value must be an XML object.", "value");
    }

    private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
    {
      List<IXmlNode> parentElements = null;

      IXmlNode parent = node;
      while ((parent = parent.ParentNode) != null)
      {
        if (parent.NodeType == XmlNodeType.Element)
        {
          if (parentElements == null)
            parentElements = new List<IXmlNode>();

          parentElements.Add(parent);
        }
      }

      if (parentElements != null)
      {
        parentElements.Reverse();

        foreach (IXmlNode parentElement in parentElements)
        {
          manager.PushScope();
          foreach (IXmlNode attribute in parentElement.Attributes)
          {
            if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/" && attribute.LocalName != "xmlns")
              manager.AddNamespace(attribute.LocalName, attribute.Value);
          }
        }
      }
    }

    private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
    {
      string prefix = (node.NamespaceURI == null || (node.LocalName == "xmlns" && node.NamespaceURI == "http://www.w3.org/2000/xmlns/"))
                        ? null
                        : manager.LookupPrefix(node.NamespaceURI);

      if (!string.IsNullOrEmpty(prefix))
        return prefix + ":" + node.LocalName;
      else
        return node.LocalName;
    }

    private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Attribute:
          if (node.NamespaceURI == JsonNamespaceUri)
            return "$" + node.LocalName;
          else
            return "@" + ResolveFullName(node, manager);
        case XmlNodeType.CDATA:
          return CDataName;
        case XmlNodeType.Comment:
          return CommentName;
        case XmlNodeType.Element:
          return ResolveFullName(node, manager);
        case XmlNodeType.ProcessingInstruction:
          return "?" + ResolveFullName(node, manager);
        case XmlNodeType.XmlDeclaration:
          return DeclarationName;
        case XmlNodeType.SignificantWhitespace:
          return SignificantWhitespaceName;
        case XmlNodeType.Text:
          return TextName;
        case XmlNodeType.Whitespace:
          return WhitespaceName;
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
      }
    }

    private bool IsArray(IXmlNode node)
    {
      IXmlNode jsonArrayAttribute = (node.Attributes != null)
                                      ? node.Attributes.SingleOrDefault(a => a.LocalName == "Array" && a.NamespaceURI == JsonNamespaceUri)
                                      : null;
      
      return (jsonArrayAttribute != null && XmlConvert.ToBoolean(jsonArrayAttribute.Value));
    }

    private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
    {
      // group nodes together by name
      Dictionary<string, List<IXmlNode>> nodesGroupedByName = new Dictionary<string, List<IXmlNode>>();

      for (int i = 0; i < node.ChildNodes.Count; i++)
      {
        IXmlNode childNode = node.ChildNodes[i];
        string nodeName = GetPropertyName(childNode, manager);

        List<IXmlNode> nodes;
        if (!nodesGroupedByName.TryGetValue(nodeName, out nodes))
        {
          nodes = new List<IXmlNode>();
          nodesGroupedByName.Add(nodeName, nodes);
        }

        nodes.Add(childNode);
      }

      // loop through grouped nodes. write single name instances as normal,
      // write multiple names together in an array
      foreach (KeyValuePair<string, List<IXmlNode>> nodeNameGroup in nodesGroupedByName)
      {
        List<IXmlNode> groupedNodes = nodeNameGroup.Value;
        bool writeArray;

        if (groupedNodes.Count == 1)
        {
          writeArray = IsArray(groupedNodes[0]);
        }
        else
        {
          writeArray = true;
        }

        if (!writeArray)
        {
          SerializeNode(writer, groupedNodes[0], manager, writePropertyName);
        }
        else
        {
          string elementNames = nodeNameGroup.Key;

          if (writePropertyName)
            writer.WritePropertyName(elementNames);

          writer.WriteStartArray();

          for (int i = 0; i < groupedNodes.Count; i++)
          {
            SerializeNode(writer, groupedNodes[i], manager, false);
          }

          writer.WriteEndArray();
        }
      }
    }

    private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Document:
        case XmlNodeType.DocumentFragment:
          SerializeGroupedNodes(writer, node, manager, writePropertyName);
          break;
        case XmlNodeType.Element:
          if (IsArray(node) && node.ChildNodes.All(n => n.LocalName == node.LocalName) && node.ChildNodes.Count > 0)
          {
            SerializeGroupedNodes(writer, node, manager, false);
          }
          else
          {
            foreach (IXmlNode attribute in node.Attributes)
            {
              if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/")
              {
                string prefix = (attribute.LocalName != "xmlns")
                                  ? attribute.LocalName
                                  : string.Empty;

                manager.AddNamespace(prefix, attribute.Value);
              }
            }

            if (writePropertyName)
              writer.WritePropertyName(GetPropertyName(node, manager));

            if (ValueAttributes(node.Attributes).Count() == 0 && node.ChildNodes.Count == 1
                && node.ChildNodes[0].NodeType == XmlNodeType.Text)
            {
              // write elements with a single text child as a name value pair
              writer.WriteValue(node.ChildNodes[0].Value);
            }
            else if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty(node.Attributes))
            {
              // empty element
              writer.WriteNull();
            }
            else
            {
              writer.WriteStartObject();

              for (int i = 0; i < node.Attributes.Count; i++)
              {
                SerializeNode(writer, node.Attributes[i], manager, true);
              }

              SerializeGroupedNodes(writer, node, manager, true);

              writer.WriteEndObject();
            }
          }

          break;
        case XmlNodeType.Comment:
          if (writePropertyName)
            writer.WriteComment(node.Value);
          break;
        case XmlNodeType.Attribute:
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.ProcessingInstruction:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          if (node.NamespaceURI == "http://www.w3.org/2000/xmlns/" && node.Value == JsonNamespaceUri)
            return;

          if (node.NamespaceURI == JsonNamespaceUri)
          {
            if (node.LocalName == "Array")
              return;
          }

          if (writePropertyName)
            writer.WritePropertyName(GetPropertyName(node, manager));
          writer.WriteValue(node.Value);
          break;
        case XmlNodeType.XmlDeclaration:
          IXmlDeclaration declaration = (IXmlDeclaration)node;
          writer.WritePropertyName(GetPropertyName(node, manager));
          writer.WriteStartObject();

          if (!string.IsNullOrEmpty(declaration.Version))
          {
            writer.WritePropertyName("@version");
            writer.WriteValue(declaration.Version);
          }
          if (!string.IsNullOrEmpty(declaration.Encoding))
          {
            writer.WritePropertyName("@encoding");
            writer.WriteValue(declaration.Encoding);
          }
          if (!string.IsNullOrEmpty(declaration.Standalone))
          {
            writer.WritePropertyName("@standalone");
            writer.WriteValue(declaration.Standalone);
          }

          writer.WriteEndObject();
          break;
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
      }
    }
    #endregion

    #region Reading
    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
      IXmlDocument document = null;
      IXmlNode rootNode = null;

#if !(UNITY_ANDROID || (UNITY_IOS || UNITY_IPHONE))
      if (typeof(XmlNode).IsAssignableFrom(objectType))
      {
        if (objectType != typeof (XmlDocument))
          throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");

        XmlDocument d = new XmlDocument();
        document = new XmlDocumentWrapper(d);
        rootNode = document;
      }
#endif
      
      if (document == null || rootNode == null)
        throw new JsonSerializationException("Unexpected type when converting XML: " + objectType);

      if (reader.TokenType != JsonToken.StartObject)
        throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");

      if (!string.IsNullOrEmpty(DeserializeRootElementName))
      {
        ReadElement(reader, document, rootNode, DeserializeRootElementName, manager);
      }
      else
      {
        reader.Read();
        DeserializeNode(reader, document, manager, rootNode);
      }

      return document.WrappedNode;
    }

    private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
    {
      switch (propertyName)
      {
        case TextName:
          currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
          break;
        case CDataName:
          currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
          break;
        case WhitespaceName:
          currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
          break;
        case SignificantWhitespaceName:
          currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
          break;
        default:
          // processing instructions and the xml declaration start with ?
          if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
          {
            CreateInstruction(reader, document, currentNode, propertyName);
          }
          else
          {
            if (reader.TokenType == JsonToken.StartArray)
            {
              // handle nested arrays
              ReadArrayElements(reader, document, propertyName, currentNode, manager);
              return;
            }

            // have to wait until attributes have been parsed before creating element
            // attributes may contain namespace info used by the element
            ReadElement(reader, document, currentNode, propertyName, manager);
          }
          break;
      }
    }

    private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new JsonSerializationException("XmlNodeConverter cannot convert JSON with an empty property name to XML.");

      Dictionary<string, string> attributeNameValues = ReadAttributeElements(reader, manager);

      string elementPrefix = MiscellaneousUtils.GetPrefix(propertyName);

      IXmlElement element = CreateElement(propertyName, document, elementPrefix, manager);

      currentNode.AppendChild(element);

      // add attributes to newly created element
      foreach (KeyValuePair<string, string> nameValue in attributeNameValues)
      {
        string attributePrefix = MiscellaneousUtils.GetPrefix(nameValue.Key);

        IXmlNode attribute = (!string.IsNullOrEmpty(attributePrefix))
                               ? document.CreateAttribute(nameValue.Key, manager.LookupNamespace(attributePrefix), nameValue.Value)
                               : document.CreateAttribute(nameValue.Key, nameValue.Value);

        element.SetAttributeNode(attribute);
      }

      if (reader.TokenType == JsonToken.String)
      {
        element.AppendChild(document.CreateTextNode(reader.Value.ToString()));
      }
      else if (reader.TokenType == JsonToken.Integer)
      {
        element.AppendChild(document.CreateTextNode(XmlConvert.ToString((long)reader.Value)));
      }
      else if (reader.TokenType == JsonToken.Float)
      {
        element.AppendChild(document.CreateTextNode(XmlConvert.ToString((double)reader.Value)));
      }
      else if (reader.TokenType == JsonToken.Boolean)
      {
        element.AppendChild(document.CreateTextNode(XmlConvert.ToString((bool)reader.Value)));
      }
      else if (reader.TokenType == JsonToken.Date)
      {
        DateTime d = (DateTime)reader.Value;
        element.AppendChild(document.CreateTextNode(XmlConvert.ToString(d, DateTimeUtils.ToSerializationMode(d.Kind))));
      }
      else if (reader.TokenType == JsonToken.Null)
      {
        // empty element. do nothing
      }
      else
      {
        // finished element will have no children to deserialize
        if (reader.TokenType != JsonToken.EndObject)
        {
          manager.PushScope();

          DeserializeNode(reader, document, manager, element);

          manager.PopScope();
        }
      }
    }

    private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
    {
      string elementPrefix = MiscellaneousUtils.GetPrefix(propertyName);

      IXmlElement nestedArrayElement = CreateElement(propertyName, document, elementPrefix, manager);

      currentNode.AppendChild(nestedArrayElement);

      int count = 0;
      while (reader.Read() && reader.TokenType != JsonToken.EndArray)
      {
        DeserializeValue(reader, document, manager, propertyName, nestedArrayElement);
        count++;
      }

      if (WriteArrayAttribute)
      {
        AddJsonArrayAttribute(nestedArrayElement, document);
      }

      if (count == 1 && WriteArrayAttribute)
      {
        IXmlElement arrayElement = nestedArrayElement.ChildNodes.CastValid<IXmlElement>().Single(n => n.LocalName == propertyName);
        AddJsonArrayAttribute(arrayElement, document);
      }
    }

    private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
    {
      element.SetAttributeNode(document.CreateAttribute("json:Array", JsonNamespaceUri, "true"));
    }

    private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
    {
      Dictionary<string, string> attributeNameValues = new Dictionary<string, string>();
      bool finishedAttributes = false;
      bool finishedElement = false;

      // a string token means the element only has a single text child
      if (reader.TokenType != JsonToken.String
          && reader.TokenType != JsonToken.Null
          && reader.TokenType != JsonToken.Boolean
          && reader.TokenType != JsonToken.Integer
          && reader.TokenType != JsonToken.Float
          && reader.TokenType != JsonToken.Date
          && reader.TokenType != JsonToken.StartConstructor)
      {
        // read properties until first non-attribute is encountered
        while (!finishedAttributes && !finishedElement && reader.Read())
        {
          switch (reader.TokenType)
          {
            case JsonToken.PropertyName:
              string attributeName = reader.Value.ToString();
              string attributeValue;

              if (!string.IsNullOrEmpty(attributeName))
              {
                char firstChar = attributeName[0];

                switch (firstChar)
                {
                  case '@':
                    attributeName = attributeName.Substring(1);
                    reader.Read();
                    attributeValue = reader.Value.ToString();
                    attributeNameValues.Add(attributeName, attributeValue);

                    string namespacePrefix;
                    if (IsNamespaceAttribute(attributeName, out namespacePrefix))
                    {
                      manager.AddNamespace(namespacePrefix, attributeValue);
                    }
                    break;
                  case '$':
                    attributeName = attributeName.Substring(1);
                    reader.Read();
                    attributeValue = reader.Value.ToString();

                    // check that JsonNamespaceUri is in scope
                    // if it isn't then add it to document and namespace manager
                    string jsonPrefix = manager.LookupPrefix(JsonNamespaceUri);
                    if (jsonPrefix == null)
                    {
                      // ensure that the prefix used is free
                      int? i = null;
                      while (manager.LookupNamespace("json" + i) != null)
                      {
                        i = i.GetValueOrDefault() + 1;
                      }
                      jsonPrefix = "json" + i;

                      attributeNameValues.Add("xmlns:" + jsonPrefix, JsonNamespaceUri);
                      manager.AddNamespace(jsonPrefix, JsonNamespaceUri);
                    }

                    attributeNameValues.Add(jsonPrefix + ":" + attributeName, attributeValue);
                    break;
                  default:
                    finishedAttributes = true;
                    break;
                }
              }
              else
              {
                finishedAttributes = true;
              }

              break;
            case JsonToken.EndObject:
              finishedElement = true;
              break;
            default:
              throw new JsonSerializationException("Unexpected JsonToken: " + reader.TokenType);
          }
        }
      }

      return attributeNameValues;
    }

    private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
    {
      if (propertyName == DeclarationName)
      {
        string version = null;
        string encoding = null;
        string standalone = null;
        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
        {
          switch (reader.Value.ToString())
          {
            case "@version":
              reader.Read();
              version = reader.Value.ToString();
              break;
            case "@encoding":
              reader.Read();
              encoding = reader.Value.ToString();
              break;
            case "@standalone":
              reader.Read();
              standalone = reader.Value.ToString();
              break;
            default:
              throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
          }
        }

        IXmlNode declaration = document.CreateXmlDeclaration(version, encoding, standalone);
        currentNode.AppendChild(declaration);
      }
      else
      {
        IXmlNode instruction = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
        currentNode.AppendChild(instruction);
      }
    }

    private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
    {
      return (!string.IsNullOrEmpty(elementPrefix))
               ? document.CreateElement(elementName, manager.LookupNamespace(elementPrefix))
               : document.CreateElement(elementName);
    }

    private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
    {
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.PropertyName:
            if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
              throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");

            string propertyName = reader.Value.ToString();
            reader.Read();

            if (reader.TokenType == JsonToken.StartArray)
            {
              int count = 0;
              while (reader.Read() && reader.TokenType != JsonToken.EndArray)
              {
                DeserializeValue(reader, document, manager, propertyName, currentNode);
                count++;
              }

              if (count == 1 && WriteArrayAttribute)
              {
                IXmlElement arrayElement = currentNode.ChildNodes.CastValid<IXmlElement>().Single(n => n.LocalName == propertyName);
                AddJsonArrayAttribute(arrayElement, document);
              }
            }
            else
            {
              DeserializeValue(reader, document, manager, propertyName, currentNode);
            }
            break;
          case JsonToken.StartConstructor:
            string constructorName = reader.Value.ToString();

            while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
            {
              DeserializeValue(reader, document, manager, constructorName, currentNode);
            }
            break;
          case JsonToken.Comment:
            currentNode.AppendChild(document.CreateComment((string)reader.Value));
            break;
          case JsonToken.EndObject:
          case JsonToken.EndArray:
            return;
          default:
            throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + reader.TokenType);
        }
      } while (reader.TokenType == JsonToken.PropertyName || reader.Read());
      // don't read if current token is a property. token was already read when parsing element attributes
    }

    /// <summary>
    /// Checks if the attributeName is a namespace attribute.
    /// </summary>
    /// <param name="attributeName">Attribute name to test.</param>
    /// <param name="prefix">The attribute name prefix if it has one, otherwise an empty string.</param>
    /// <returns>True if attribute name is for a namespace attribute, otherwise false.</returns>
    private bool IsNamespaceAttribute(string attributeName, out string prefix)
    {
      if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
      {
        if (attributeName.Length == 5)
        {
          prefix = string.Empty;
          return true;
        }
        else if (attributeName[5] == ':')
        {
          prefix = attributeName.Substring(6, attributeName.Length - 6);
          return true;
        }
      }
      prefix = null;
      return false;
    }

    private IEnumerable<IXmlNode> ValueAttributes(IEnumerable<IXmlNode> c)
    {
      return c.Where(a => a.NamespaceURI != JsonNamespaceUri);
    }
    #endregion

    /// <summary>
    /// Determines whether this instance can convert the specified value type.
    /// </summary>
    /// <param name="valueType">Type of the value.</param>
    /// <returns>
    ///     <c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type valueType)
    {
#if !(UNITY_ANDROID || (UNITY_IOS || UNITY_IPHONE))
      if (typeof(XmlNode).IsAssignableFrom(valueType))
        return true;
#endif

      return false;
    }
  }
}
#endif
#endif