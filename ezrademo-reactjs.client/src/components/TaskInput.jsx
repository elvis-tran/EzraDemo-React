import React, { useState } from 'react';

function TaskInput({ onTaskCreated }) {
    const [input, setInput] = useState('');
    const [error, setError] = useState('');

    const isValid = input.trim().length > 0;

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!isValid) {
            setError('Task name cannot be empty.');
            return;
        }
        setError('');
        await onTaskCreated({ taskName: input.trim() });
        setInput('');
    };

    const handleChange = (e) => {
        setInput(e.target.value);
        if (error && e.target.value.trim().length > 0) {
            setError('');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="Enter task name"
                value={input}
                onChange={handleChange}
                aria-label="Task name"
            />
            <button type="submit" disabled={!isValid}>
                Add Task
            </button>
            {error && <p style={{ color: 'red', marginTop: '0.5rem' }}>{error}</p>}
        </form>
    );
}

export default TaskInput;