
import {
  Navigate
} from "react-router-dom";

import {
  getToken
} from "../storage/auth";

type Props = {
  children: React.ReactNode;
};

export default function PrivateRoute({
  children
}: Props){

  const token =
    getToken();

  if(!token){

    return (
      <Navigate
        to="/"
        replace
      />
    );
  }

  return children;
}

