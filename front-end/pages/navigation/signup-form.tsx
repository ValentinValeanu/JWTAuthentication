"use client";

import { useRef, useState, useEffect, useContext } from "react";

import { SignupUserFormData } from "@/entities/form-types";
import { MenuContext } from "./menu-context";

export default function SignUpForm() {
  const context = useContext(MenuContext);

  if (!context) {
    throw new Error("Child must be used within a MenuProvider.");
  }

  const { setIsSignUpFormVisible } = context;

  const [signupUserFormData, setSignupUserFormData] =
    useState<SignupUserFormData>({
      firstName: "",
      lastName: "",
      email: "",
      password: "",
    });

  const mainDivRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        mainDivRef.current &&
        !mainDivRef.current.contains(event.target as Node)
      ) {
        setIsSignUpFormVisible(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setSignupUserFormData((prev) => ({
      ...prev,
      [name]: value || undefined,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const response = await fetch("https://localhost:7023/api/auth/signup", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(signupUserFormData),
    });

    setIsSignUpFormVisible(false);
  };

  return (
    <div
      ref={mainDivRef}
      className="absolute right-0 top-full mt-2 w-100 rounded-lg text-black border bg-white p-4 shadow-lg"
    >
      <form className="flex flex-col gap-2">
        <input
          placeholder="First Name"
          type="text"
          name="firstName"
          className="border rounded px-3 py-2"
          value={signupUserFormData.firstName}
          onChange={handleChange}
        />

        <input
          placeholder="Last Name"
          type="text"
          name="lastName"
          className="border rounded px-3 py-2"
          value={signupUserFormData.lastName}
          onChange={handleChange}
        />

        <input
          placeholder="Last Name"
          type="date"
          name="birthdate"
          className="border rounded px-3 py-2"
          value={signupUserFormData?.birthdate}
          onChange={handleChange}
        />

        <input
          className="border rounded px-3 py-2"
          placeholder="Email"
          type="email"
          name="email"
          value={signupUserFormData.email}
          onChange={handleChange}
        />

        <input
          type="password"
          name="password"
          value={signupUserFormData.password}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Password"
        />
        <button
          className="bg-black text-white rounded py-2"
          onClick={handleSubmit}
        >
          Create Account
        </button>
      </form>
    </div>
  );
}
