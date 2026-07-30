import { Navigate } from "react-router-dom";
import { getToken } from "../storage/auth";
import { getRoleFromToken } from "../utils/jwt";

type Props = {
  children: React.ReactNode;
  role: string;
};

export default function RoleRoute({
  children,
  role
}: Props) {

  const token = getToken();

  if (!token) {
    return <Navigate to="/" replace />;
  }

  const userRole =
    getRoleFromToken(token);

  console.log("TOKEN SALVO:", token);
  console.log("ROLE TOKEN:", userRole);
  console.log("ROLE NECESSÁRIA:", role);

  

  if (!userRole) {
    return <Navigate to="/" replace />;
  }

  if (
    userRole.toLowerCase() !==
    role.toLowerCase()
  ) {
    return <Navigate to="/" replace />;
  }

  return children;
}