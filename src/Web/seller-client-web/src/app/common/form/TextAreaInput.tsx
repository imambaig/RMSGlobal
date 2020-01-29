import React from 'react'
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, FormField, Label } from 'semantic-ui-react'

interface IPros extends FieldRenderProps<string, HTMLElement>, FormFieldProps { }
const TextAreaInput: React.FC<IPros> = ({ input, width, rows, placeholder, meta: { touched, error } }) => {
    return (
        <FormField error={touched && !!error} width={width}>
            <textarea rows={rows} {...input} placeholder={placeholder} />
            {touched && error && (
                <Label basic color='red'>
                    {error}
                </Label>
            )}
        </FormField>
    )
}
export default TextAreaInput
