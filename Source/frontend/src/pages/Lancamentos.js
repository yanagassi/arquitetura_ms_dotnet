import React, { useState } from "react";

import Helper from "../helpers/Helper";
import { NumericFormat } from "react-number-format";

function Lancamentos() {
  const [formData, setFormData] = useState({
    data: Helper.getCurrentDate(),
    descricao: "",
    tipo: "debito",
    valor: 0,
    categoria: "",
    meioPagamento: "",
    observacoes: "",
    conta: "Conta A",
  });

  const categoriasPorTipo = {
    debito: [
      "Compras de Estoques",
      "Despesas Operacionais",
      "Pagamentos a Fornecedores",
      "Investimentos",
      "Empréstimos e Financiamentos",
      "Impostos",
      "Retiradas do Proprietário",
      "Outras Despesas",
    ],
    credito: [
      "Vendas",
      "Recebimentos de Clientes",
      "Investimentos",
      "Empréstimos",
      "Outras Receitas",
    ],
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (name === "tipo") {
      setFormData({
        ...formData,
        [name]: value,
        categoria: categoriasPorTipo[value][0], // Define a primeira categoria por padrão
      });
    } else if (name === "valor") {
      // Remove caracteres não numéricos e converte para número
      const numericValue = parseFloat(value.replace(/[^0-9.-]/g, ""));

      setFormData({
        ...formData,
        [name]: numericValue,
      });
    } else {
      setFormData({
        ...formData,
        [name]: value,
      });
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Lógica para lidar com o envio do formulário (por exemplo, salvar os dados no estado global, enviar para um servidor, etc.)
    console.log(formData);
  };

  return (
    <div className="container mx-auto p-14 pt-0">
      <form onSubmit={handleSubmit} className="">
        <h2 className="text-2xl font-semibold mb-8">Lançamentos</h2>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
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
              name="descricao"
              value={formData.descricao}
              onChange={handleChange}
              className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
            />
          </div>
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
              id="conta"
              name="conta"
              value={formData.conta}
              required={true}
              onChange={handleChange}
              className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
            >
              <option value="Conta A">Conta A</option>
              <option value="Conta B">Conta B</option>
            </select>
          </div>
          <div className="mb-4">
            <label
              htmlFor="categoria"
              className="block text-sm font-medium text-gray-600"
            >
              Categoria *
            </label>
            <select
              id="categoria"
              name="categoria"
              value={formData.categoria}
              onChange={handleChange}
              className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
            >
              {categoriasPorTipo[formData.tipo].map((categoria) => (
                <option key={categoria} value={categoria}>
                  {categoria}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="mb-4 col-span-2">
          <label
            htmlFor="observacoes"
            className="block text-sm font-medium text-gray-600"
          >
            Observações
          </label>
          <textarea
            id="observacoes"
            name="observacoes"
            value={formData.observacoes}
            onChange={handleChange}
            className="mt-1 p-2 w-full border  border-stone-300 rounded-md focus:outline-none focus:border-primary"
          />
        </div>

        <div className="mt-6 w-full flex justify-end">
          <button
            type="submit"
            className="py-2 px-4 bg-blue-500 text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring focus:border-blue-300"
          >
            Salvar
          </button>
        </div>
      </form>
    </div>
  );
}

export default Lancamentos;
