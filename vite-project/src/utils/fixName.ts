export function fixName(text: string) {
  if (!text) return "";

  try {
    return decodeURIComponent(escape(text));
  } catch {
    return text;
  }
}

