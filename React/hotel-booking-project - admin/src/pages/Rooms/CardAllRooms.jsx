import React from 'react';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import CardActions from '@mui/material/CardActions';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';



const CardAllRooms = ({roomId, roomType, roomNumber, price, floor, description, imageURL, onDelete }) => {
    return (
        <Card sx={{ maxWidth: 345 }}>
            <CardMedia
                component="img"
                height="140"
                image={imageURL}
                alt={roomType}
            />
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {roomType} Værelse
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    {description}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Etage: {floor}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Værelsesnummer: {roomNumber ? roomNumber : 'will be assigned'}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Pris: {price} DKK pr. nat
                </Typography>
            </CardContent>
            <CardActions>
                <Button onClick={() => onDelete(roomId)} size="small" color="primary">
                    Slet
                </Button>
            </CardActions>
        </Card>
    );
};

export default CardAllRooms;