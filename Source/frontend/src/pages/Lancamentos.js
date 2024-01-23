import React, { useState } from "react";

import Helper from "../helpers/Helper";
import { NumericFormat } from "react-number-format";
import LancamentosService from "../services/LancamentosService";
import { useApi } from "../context/ApiContext";
import { toast } from "react-toastify";

const initialState = {
  data: Helper.getCurrentDate(),
  descricao: "",
  tipo: "debito",
  valor: 0,
  contaId: "89bc5c51-016e-4d57-a6bd-f884dedf98a8",
};

function Lancamentos() {
  const { user, setLoad } = useApi();
  const [formData, setFormData] = useState(initialState);

  const handleChange = (e) => {
    const { name, value } = e.target;

    setFormData((prevData) => ({
      ...prevData,
      [name]:
        name === "valor" ? parseFloat(value.replace(/[^0-9.-]/g, "")) : value,
    }));
  };

  const validateForm = () => {
    if (!formData.descricao.trim()) {
      toast.error("Descrição é obrigatória");
      return false;
    }

    if (!formData.data) {
      toast.error("Data é obrigatória");
      return false;
    }

    if (formData.valor <= 0) {
      toast.error("Valor deve ser maior que zero");
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    try {
      setLoad(true);
      await LancamentosService.adicionarLancamento({
        ...formData,
        userId: user?.nameidentifier,
        data: new Date(formData.data).toISOString(),
      });
      toast.success("Lançamento adicionado com sucesso!");
      setFormData(initialState);
      setLoad(false);
    } catch (error) {
      toast.error(error);
      setLoad(false);
    }
  };
  return (
    <div className="container mx-auto p-14 pt-0">
      <form onSubmit={handleSubmit} className="">
        <h2 className="text-2xl font-semibold mb-8 text-gray">Lançamentos</h2>
        <div className="mb-4">
          <label
            htmlFor="descricao"
            className="block text-sm font-medium text-gray-600"
          >
            Descrição *
          </label>
          <input
            type="text"
            id="descricao"
            required
            name="descricao"
            value={formData.descricao}
            onChange={handleChange}
            className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
          />
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
          <div className="mb-4">
            <label
              htmlFor="data"
              className="block text-sm font-medium text-gray-600"
            >
              Data *
            </label>
            <input
              type="date"
              id="data"
              name="data"
              required={true}
              value={formData.data}
              onChange={handleChange}
              className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:none focus:border-primary"
            />
          </div>

          <div className="mb-4">
            <label
              htmlFor="valor"
              className="block text-sm font-medium text-gray-600"
            >
              Valor *
            </label>
            <NumericFormat
              thousandSeparator=","
              decimalSeparator="."
              prefix="R$ "
              id="valor"
              required={true}
              name="valor"
              value={formData.valor}
              onChange={handleChange}
              className="mt-1 p-2 w-full border h-[38px]  border-stone-300 rounded-md focus:outline-none focus:border-primary"
              decimalScale={2}
            />
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
            <div className="mb-4 col-span-2">
              <label
                htmlFor="conta"
                className="block text-sm font-medium text-gray-600"
              >
                Tipo *
              </label>
              <select
                id="tipo"
                name="tipo"
                required={true}
                value={formData.tipo}
                onChange={handleChange}
                className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
              >
                <option value="debito">Débito</option>
                <option value="credito">Crédito</option>
              </select>
            </div>
          </div>
          <div className="mb-4">
            <label
              htmlFor="conta"
              className="block text-sm font-medium text-gray-600"
            >
              Conta *
            </label>
            <select
              id="contaId"
              name="contaId"
              value={formData.contaId}
              required={true}
              onChange={handleChange}
              className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
            >
              <option value="89bc5c51-016e-4d57-a6bd-f884dedf98a8">
                Conta Única
              </option>
            </select>
          </div>
        </div>

        <div className="mt-6 w-full flex justify-end">
          <button
            type="submit"
            className="py-2 px-4 bg-primary text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring focus:border-blue-300"
          >
            Salvar
          </button>
        </div>
      </form>
    </div>
  );
}

export default Lancamentos;
