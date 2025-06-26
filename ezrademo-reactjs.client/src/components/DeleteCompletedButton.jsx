import React, { useState, useEffect } from 'react';

function DeleteCompletedButton({ onDeleteCompleted, hasCompletedTasks, resetSignal }) {
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState(null);
    const [error, setError] = useState(null);

    useEffect(() => {
        // Clear messages whenever resetSignal changes
        setMessage(null);
        setError(null);
    }, [resetSignal]);

    const handleClick = async () => {
        setLoading(true);
        setError(null);
        setMessage(null);

        try {
            const response = await onDeleteCompleted();
            if (response?.deleted > 0) {
                setMessage(`Deleted ${response.deleted} completed task(s).`);
            } else {
                setMessage('No completed tasks to delete.');
            }
        } catch (err) {
            const errMsg =
                err.response?.data ||
                err.message ||
                'Failed to delete completed tasks.';
            setError(errMsg);
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ marginBottom: '1rem' }}>
            <button onClick={handleClick} disabled={loading || !hasCompletedTasks}>
                {loading ? 'Deleting...' : 'Delete Completed Tasks'}
            </button>
            {message && <p style={{ color: 'green', marginTop: '0.5rem' }}>{message}</p>}
            {error && <p style={{ color: 'red', marginTop: '0.5rem' }}>{error}</p>}
        </div>
    );
}

export default DeleteCompletedButton;