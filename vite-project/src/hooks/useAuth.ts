import { useNavigate }
from "react-router-dom";

import {
  getToken,
  logout
} from "../storage/auth";

export function useAuth(){

  const navigate =
    useNavigate();

  function isAuthenticated(){
    return !!getToken();
  }

  function signOut(){
    logout();

    navigate("/login");
  }

  return{
    isAuthenticated,
    signOut
  };
}