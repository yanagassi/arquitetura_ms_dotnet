import React from "react";
import { render } from "@testing-library/react";
import HeaderPage from "../components/HeaderPage";

describe("HeaderPage", () => {
  it("renderiza com o título e descrição corretos", () => {
    const title = "Título de Teste";
    const description = "Descrição de Teste";
    const { getByText } = render(
      <HeaderPage title={title} description={description} />
    );

    const titleElement = getByText(title);
    const descriptionElement = getByText(description);

    expect(titleElement).toBeInTheDocument();
    expect(descriptionElement).toBeInTheDocument();
  });

  it("aplica os estilos corretos", () => {
    const title = "Título de Teste";
    const description = "Descrição de Teste";
    const { getByText } = render(
      <HeaderPage title={title} description={description} />
    );

    const titleElement = getByText(title);
    const descriptionElement = getByText(description);

    expect(titleElement).toHaveClass("text-2xl font-semibold text-gray");
    expect(descriptionElement).toHaveClass("text-slate-400");
  });
});
