import React from 'react';
import TaskItem from './TaskItem';

function TaskList({ tasks, onToggle }) {
    if (tasks.length === 0) return <p>No tasks yet.</p>;

    return (
        <ul style={{ listStyleType: 'none', paddingLeft: 0 }}>
            {tasks.map((task) => (
                <TaskItem key={task.id} task={task} onToggle={onToggle} />
            ))}
        </ul>
    );

}

export default TaskList;