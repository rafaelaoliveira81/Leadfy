import { useState, useCallback, useEffect } from 'react';
import interactionApi from '../../../../services/interactionApi';
import opportunityAPI from '../../../../services/opportunityApi';

export function useInteractions(opportunityId, open) {
    const [interactions, setInteractions] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [historyOpen, setHistoryOpen] = useState(false);
    const [addModalOpen, setAddModalOpen] = useState(false);
    const [detailModalData, setDetailModalData] = useState(null);

    // Limpa interações ao reabrir o modal para forçar re-fetch no próximo expand
    useEffect(() => {
        if (open) {
            setInteractions([]);
        }
    }, [open]);

    const fetchInteractions = useCallback(async () => {
        if (!opportunityId) return;
        setIsLoading(true);
        try {
            const data = await interactionApi.GetByOpportunityId(opportunityId);
            setInteractions(Array.isArray(data) ? data : []);
        } catch {
            setInteractions([]);
        } finally {
            setIsLoading(false);
        }
    }, [opportunityId]);

    const openHistory = useCallback(() => {
        setHistoryOpen(true);
        fetchInteractions();
    }, [fetchInteractions]);

    const closeHistory = useCallback(() => setHistoryOpen(false), []);

    const openAddModal = useCallback(() => setAddModalOpen(true), []);
    const closeAddModal = useCallback(() => setAddModalOpen(false), []);

    const openDetailModal = useCallback((interaction) => setDetailModalData(interaction), []);
    const closeDetailModal = useCallback(() => setDetailModalData(null), []);

    const addInteraction = useCallback(
        async ({ description, toStage, userId }) => {
            await interactionApi.AddToOpportunity(opportunityId, {
                description,
                toStage: Number(toStage),
                userId: Number(userId),
                interactionDate: new Date().toISOString(),
            });
            // Change opportunity stage
            await opportunityAPI.PatchStage(opportunityId, Number(toStage));
            closeAddModal();
        },
        [opportunityId, closeAddModal]
    );

    return {
        interactions,
        isLoading,
        historyOpen,
        addModalOpen,
        detailModalData,
        openHistory,
        closeHistory,
        openAddModal,
        closeAddModal,
        openDetailModal,
        closeDetailModal,
        addInteraction,
        fetchInteractions,
    };
}
