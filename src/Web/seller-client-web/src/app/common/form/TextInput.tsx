﻿import React from 'react'
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, FormField, Label } from 'semantic-ui-react'

interface IProps extends FieldRenderProps<string, HTMLElement>, FormFieldProps { }

const TextInput: React.FC<IProps> = ({ input, width, type, placeholder, meta: { touched, error } }) => {
    return <FormField error={touched && !!error} type={type} width={width}>
        <input {...input} placeholder={placeholder} type="text" />
        {touched && error && (
            <Label basic color='red'>
                {error}
            </Label>
        )}
    </FormField>
}

export default TextInput