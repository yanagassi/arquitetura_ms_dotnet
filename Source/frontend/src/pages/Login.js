import React, { useState } from "react";
import { useApi } from "../context/ApiContext";

const Login = () => {
  const { login } = useApi();
  const [username, setUsername] = useState("admin@admin.com");
  const [password, setPassword] = useState("admin");

  const handleLogin = async () => {
    await login(username, password);
  };

  return (
    <div className="flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded shadow-md w-96 mt-52">
        <h2 className="text-2xl font-semibold mb-6">Login</h2>
        <div className="mb-4">
          <label
            htmlFor="username"
            className="block text-sm font-medium text-gray-600"
          >
            Email
          </label>
          <input
            type="text"
            id="username"
            value={username}
            className="mt-1 p-2 w-full border border-gray rounded-sm"
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div className="mb-4">
          <label
            htmlFor="password"
            className="block text-sm font-medium text-gray-600"
          >
            Password
          </label>
          <input
            type="password"
            id="password"
            value={password}
            className="mt-1 p-2 w-full border rounded-sm"
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button
          className="bg-blue-500 text-white p-2 rounded-sm hover:bg-blue-600 focus:outline-none focus:ring focus:border-blue-300"
          onClick={handleLogin}
        >
          Vamos Lá!
        </button>
      </div>
    </div>
  );
};

export default Login;
