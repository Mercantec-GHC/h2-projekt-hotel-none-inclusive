import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography, Button, TextField } from '@mui/material';


function UsersPage() {
    const [users, setUsers] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        fetch('https://localhost:7207/api/Users')
            .then(response => response.json())
            .then(data => setUsers(data))
            .catch(error => console.error('Error fetching users:', error));
    }, []);

    const deleteUser = (userId) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            fetch(`https://localhost:7207/api/Users/${userId}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        setUsers(users.filter(user => user.id !== userId));
                    } else {
                        console.error('Error deleting user:', response.statusText);
                    }
                })
                .catch(error => console.error('Error deleting user:', error));
        }
    };

    const filteredUsers = users.filter(user =>
        user.email.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <div className="users-container">
            <h1>Users</h1>
            <TextField
                label="Search by email"
                variant="outlined"
                fullWidth
                margin="normal"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
            />
            <div className="users-list">
                {filteredUsers.map(user => (
                    <Card key={user.id} sx={{marginBottom: 2}}>
                        <CardContent>
                            <Typography variant="h5" component="div">
                                {user.name}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Email: {user.email}
                            </Typography>
                            <Button
                                variant="contained"
                                color="secondary"
                                onClick={() => deleteUser(user.id)}
                            >
                                Delete
                            </Button>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
}

export default UsersPage;