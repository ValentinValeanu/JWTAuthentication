"use client";
import LoginForm from "./login-form";
import SignUpForm from "./signup-form";
import { useState } from "react";
import { MenuContext } from "./menu-context";

export default function Menu() {
  const [isLogInFormVisible, setIsLogInFormVisible] = useState<boolean>(false);
  const [isSignUpFormVisible, setIsSignUpFormVisible] =
    useState<boolean>(false);

  return (
    <MenuContext.Provider
      value={{
        isLogInFormVisible,
        setIsLogInFormVisible,
        isSignUpFormVisible,
        setIsSignUpFormVisible,
      }}
    >
      <div className="relative flex gap-3">
        <button
          onClick={() => setIsLogInFormVisible(true)}
          className={`px-4 py-2 ${isLogInFormVisible ? "text-gray-300" : "text-white"}`}
        >
          Log In
        </button>

        <button
          className={`px-4 py-2 ${isSignUpFormVisible ? "text-gray-300" : "text-white"}`}
          onClick={() => setIsSignUpFormVisible(true)}
        >
          Sign Up
        </button>

        {isLogInFormVisible && <LoginForm />}
        {isSignUpFormVisible && <SignUpForm />}
      </div>
    </MenuContext.Provider>
  );
}
