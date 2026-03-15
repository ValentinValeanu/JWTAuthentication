"use client";

import { useRef, useState, useEffect, useContext } from "react";

import { LoginUserFormData } from "@/entities/form-types";
import { MenuContext } from "./menu-context";

export default function LoginForm() {
  const context = useContext(MenuContext);

  if (!context) {
    throw new Error("Child must be used within a MenuProvider.");
  }

  const { setIsLogInFormVisible } = context;

  const [loginUserFormData, setLoginUserFormData] = useState<LoginUserFormData>(
    {
      email: "",
      password: "",
    },
  );

  const mainDivRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        mainDivRef.current &&
        !mainDivRef.current.contains(event.target as Node)
      ) {
        setIsLogInFormVisible(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setLoginUserFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const response = await fetch("https://localhost:7023/api/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(loginUserFormData),
    });

    //const data = await response.json();

    setIsLogInFormVisible(false);
  };

  return (
    <div
      ref={mainDivRef}
      className="absolute right-0 top-full mt-2 w-100 rounded-lg text-black border bg-white p-4 shadow-lg"
    >
      <form className="flex flex-col gap-2">
        <input
          type="email"
          name="email"
          value={loginUserFormData.email}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Email"
        />

        <input
          type="password"
          name="password"
          value={loginUserFormData.password}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Password"
        />

        <button
          className="bg-black text-white rounded py-2"
          type="submit"
          onClick={handleSubmit}
        >
          Log In
        </button>
      </form>
    </div>
  );
}
