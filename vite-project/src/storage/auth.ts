export function saveAuth(
  token: string,
  refreshToken: string,
  user: any
) {
  localStorage.setItem("token", token);
  localStorage.setItem("refreshToken", refreshToken);
  localStorage.setItem("user", JSON.stringify(user));
}

export function logout(){
  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");
  localStorage.removeItem("user");
}

export function getToken(){
  return localStorage.getItem("token");
}

export function getUser(){
  const user = localStorage.getItem("user");

  if(!user) return null;

  return JSON.parse(user);
}