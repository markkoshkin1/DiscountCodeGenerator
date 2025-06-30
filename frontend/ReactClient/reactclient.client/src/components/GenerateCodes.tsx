import React, { useState } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  RadioGroup,
  FormControlLabel,
  Radio,
  FormLabel,
} from '@mui/material';

export default function GenerateCodes() {
  const [length, setLength] = useState<'7' | '8'>('7');
  const [amount, setAmount] = useState<number>(1);
  const [generatedCodes, setGeneratedCodes] = useState<string[]>([]);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleGenerate = async () => {
    setError('');
    setGeneratedCodes([]);

    if (amount < 1 || amount > 2000) {
      setError('Amount must be between 1 and 2000');
      return;
    }

    setLoading(true);
    try {
      const response = await fetch(
        `https://localhost:7017/api/discountcodes/generate?length=${length}&amount=${amount}`
      );

      if (!response.ok) {
        const errText = await response.text();
        throw new Error(errText || 'Failed to generate codes');
      }

      const data = await response.json();

      if (data) {
        setGeneratedCodes(data);
      } else {
        setError('Invalid response from server');
      }
    } catch (ex: any) {
      setError(ex.message || 'An error occurred');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Paper sx={{ padding: 3 }}>
      <Typography variant="h6" gutterBottom>
        Generate Codes
      </Typography>

      <FormLabel component="legend">Code Length</FormLabel>
      <RadioGroup
        row
        value={length}
        onChange={(e) => setLength(e.target.value as '7' | '8')}
        sx={{ mb: 2 }}
      >
        <FormControlLabel value="7" control={<Radio />} label="7" />
        <FormControlLabel value="8" control={<Radio />} label="8" />
      </RadioGroup>

      <TextField
        label="Amount"
        type="number"
        fullWidth
        margin="normal"
        value={amount}
        onChange={(e) => setAmount(Number(e.target.value))}
        inputProps={{ min: 1, max: 2000 }}
        error={!!error}
        helperText={error}
      />

      <Button
        variant="contained"
        fullWidth
        onClick={handleGenerate}
        sx={{ mt: 2 }}
        disabled={loading}
      >
        {loading ? 'Generating...' : 'Generate'}
      </Button>

      <TextField
        label="Generated Codes"
        multiline
        fullWidth
        rows={8}
        margin="normal"
        value={generatedCodes.join('\n')}
        InputProps={{ readOnly: true }}
      />
    </Paper>
  );
}
