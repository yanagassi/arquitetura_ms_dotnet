import React from "react";
import { render, screen } from "@testing-library/react";
import LoadSpinner from "../components/LoadSpinner";

describe("LoadSpinner", () => {
  it("renderiza o spinner quando isLoading é verdadeiro", () => {
    const { container } = render(<LoadSpinner isLoading={true} />);
    const spinnerElement = container.querySelector(".animate-spin");
    expect(spinnerElement).toBeInTheDocument();
  });

  it("não renderiza o spinner quando isLoading é falso", () => {
    render(<LoadSpinner isLoading={false} />);
    const spinnerElement = screen.queryByTestId("spinner");
    expect(spinnerElement).not.toBeInTheDocument();
  });

  it("renderiza sem falhas com isLoading indefinido", () => {
    render(<LoadSpinner />);
    const spinnerElement = screen.queryByTestId("spinner");
    expect(spinnerElement).not.toBeInTheDocument();
  });
});
