import { useEffect, useState } from 'react';
import './App.css';
import { Container, Grid } from '@mui/material';
import GenerateCodes from './components/GenerateCodes';
import UseCode from './components/UseCode';

function App() {
  const [codes, setCodes] = useState<string[]>([]);

    return (
<Container maxWidth="xl" sx={{ mt: 4 }}>
<Grid container spacing={2}>
   <Grid size={6}>
    <GenerateCodes />
   </Grid>
   <Grid size={6}>
    <UseCode />
   </Grid>
  </Grid>
    </Container>
  );
}

export default App;