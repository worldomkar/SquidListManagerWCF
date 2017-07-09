//-----------------------------------------------------------------------
// <copyright file="Section.cs" company="_">
// Section
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Section contains two sub lists
    /// <para>First one is list of active domains and Second one is list of inactive domains</para>
    /// <para>There can be more than one Sections in a DomainsList</para>
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
            this.ActiveDomain = new List<string>();
            this.InactiveDomain = new List<string>();
        }

        /// <summary>
        /// Gets or sets Name of the Section
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets List of active domains
        /// </summary>
        [DataMember]
        public List<string> ActiveDomain { get; set; }

        /// <summary>
        /// Gets or sets List of inactive domains
        /// </summary>
        [DataMember]
        public List<string> InactiveDomain { get; set; }
    }
}
