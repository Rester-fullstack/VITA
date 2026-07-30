export function decodeJwt(token: string) {
  try {
    if (!token || typeof token !== "string") return null;

    const parts = token.split(".");
    if (parts.length !== 3) return null;

    const base64Url = parts[1];
    if (!base64Url) return null;

    const base64 = base64Url
      .replace(/-/g, "+")
      .replace(/_/g, "/");

    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );

    return JSON.parse(jsonPayload);
  } catch (error) {
    console.log("Erro ao decodificar token:", error);
    return null;
  }
}

export function getRoleFromToken(token: string) {
  const payload = decodeJwt(token);

  if (!payload) return null;

  return (
    payload[
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ] ?? null
  );
}