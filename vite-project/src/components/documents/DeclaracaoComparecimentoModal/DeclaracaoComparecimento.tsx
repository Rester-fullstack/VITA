import { useState } from "react";

import toast from "react-hot-toast";

import api from "../../../api/axios";

import "./DeclaracaoComparecimento.css";

type Props = {
    open: boolean;
    consultaId: number;
    onClose: () => void;
    onSuccess: () => void;
};

export default function DeclaracaoComparecimentoModal({
    open,
    consultaId,
    onClose,
    onSuccess
}: Props) {

    const [observacoes, setObservacoes] =
        useState("");

    const [loading, setLoading] =
        useState(false);

    if (!open) return null;

    async function emitir() {

        try {

            setLoading(true);

            await api.post(
                "/DeclaracaoComparecimento",
                {
                    consultaId,
                    observacoes
                }
            );

            toast.success(
                "Declaração emitida com sucesso."
            );

            onSuccess();

            onClose();

        } catch {

            toast.error(
                "Erro ao emitir declaração."
            );

        } finally {

            setLoading(false);

        }

    }

    return (

        <div className="modal-overlay">

            <div className="declaracao-modal">

                <h2>
                    Declaração de Comparecimento
                </h2>

                <p>
                    Emitir declaração desta consulta.
                </p>

                <label>

                    Observações

                </label>

                <textarea

                    value={observacoes}

                    onChange={(e) =>
                        setObservacoes(
                            e.target.value
                        )
                    }

                    placeholder="Observações (opcional)"

                />

                <div className="modal-actions">

                    <button

                        className="cancel-btn"

                        onClick={onClose}

                    >

                        Cancelar

                    </button>

                    <button

                        className="save-btn"

                        onClick={emitir}

                    >

                        {
                            loading
                                ? "Emitindo..."
                                : "Emitir Declaração"
                        }

                    </button>

                </div>

            </div>

        </div>

    );

}