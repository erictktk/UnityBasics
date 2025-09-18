// TagAliases.cs
using System.Collections.Generic;

public static class TagAliases {
    // Base → all acceptable forms
    public static readonly Dictionary<string, HashSet<string>> MAP = new Dictionary<string, HashSet<string>> {
        { "move", new HashSet<string>{ "move","moves","moving","moved","movement","mover" } },
        { "rotate", new HashSet<string>{ "rotate","rotates","rotating","rotated","rotation","rotator" } },
        { "scale", new HashSet<string>{ "scale","scales","scaling","scaled" } },
        { "toggle", new HashSet<string>{ "toggle","toggles","toggling","toggled" } },
        { "on off", new HashSet<string>{ "toggle" } },
        { "color", new HashSet<string>{ "color","colors","coloring","coloured","colour","hsl","hsv","hue" } },
        { "ui", new HashSet<string>{ "ui","u.i.","userinterface","interface" } },
        { "camera", new HashSet<string>{ "camera","cameras","cam","billboard","follow" } },
        { "audio", new HashSet<string>{ "audio","sound","sfx","pitch" } },
        { "material", new HashSet<string>{ "material","materials","shader","uv","texture","tex" } },
        { "editor", new HashSet<string>{ "editor","inspector","drawer","menu","window","tool" } },
        { "animation", new HashSet<string>{ "animation","animate","animating","animated","anim" } },
        { "mover", new HashSet<string>{ "movement", "move" } },
        { "tool", new HashSet<string> { "tools", "editor" } },
        { "game", new HashSet<string> { } },
        { "health", new HashSet<string> {"game" } },
        { "typing", new HashSet<string> {"ui", "dialogue", "type", "text" } },
        { "align", new HashSet<string> {"alignment" } },
        { "parameter", new HashSet<string> {"driver", "parameters" } },
        { "hierarchy", new HashSet<string> {"tool", "tools", "organization" } },
        { "organization", new HashSet<string> {"organize" } },
        { "spawn", new HashSet<string> {"spawner", "spawning" } },
        { "place", new HashSet<string> {"placer", "placing", "scene" } },
    };

    // Expand a query term to all aliases (fallback to itself).
    public static IEnumerable<string> Expand(string term) =>
        MAP.TryGetValue(term.ToLowerInvariant(), out var set) ? set : new HashSet<string> { term.ToLowerInvariant() };
}