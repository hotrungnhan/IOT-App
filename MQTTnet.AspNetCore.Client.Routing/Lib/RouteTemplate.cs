using System.Diagnostics;
using Microsoft.AspNetCore.Routing.Template;

namespace MQTTnet.AspNetCore.Client.Routing.Lib;

[DebuggerDisplay("{TemplateText}")]
public class RouteTemplate : IEquatable<RouteTemplate>
{
    public RouteTemplate(string templateText, List<TemplateSegment> segments)
    {
        if (segments == null)
        {
            throw new ArgumentNullException(nameof(segments));
        }

        TemplateText = templateText;
        Segments = segments;
        OptionalSegmentsCount = segments.Count(template => template.IsOptional);
        ContainsCatchAllSegment = segments.Any(template => template.IsCatchAll);
    }

    public string TemplateText { get; }

    public IList<TemplateSegment> Segments { get; }

    public int OptionalSegmentsCount { get; }

    public bool ContainsCatchAllSegment { get; }

    public bool Equals(RouteTemplate other)
    {
        if (other == null)
        {
            return false;
        }

        if (!string.Equals(TemplateText, other.TemplateText, StringComparison.Ordinal))
        {
            return false;
        }

        if (Segments.Count != other.Segments.Count)
        {
            return false;
        }

        for (int i = 0; i < Segments.Count; i++)
        {
            if (!Segments[i].Equals(other.Segments[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object other)
    {
        if (!this.GetType().Equals(other.GetType()))
        {
            return false;
        }
        else
        {
            return Equals((RouteTemplate)other);
        }
    }

    public override int GetHashCode()
    {
        return TemplateText.GetHashCode();
    }
}