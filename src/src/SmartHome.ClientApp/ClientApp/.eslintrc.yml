env:
    browser: true
    es2022: true
extends:
    - plugin:vue/vue3-recommended
    - standard-with-typescript
    - plugin:import/recommended
    - plugin:import/typescript
    - plugin:vuetify/base
    - '@vue/eslint-config-prettier'
overrides: []
parser: vue-eslint-parser
parserOptions:
    ecmaVersion: latest
    parser: '@typescript-eslint/parser'
    sourceType: module
    createDefaultProgram: true
    project:
        - ./tsconfig.app.json
        - ./tsconfig.node.json
        - ./tsconfig.vitest.json
    extraFileExtensions:
        - .vue
plugins:
    - import
    - vue
rules:
    require-jsdoc: off
    no-unused-vars: warn
    '@typescript-eslint/array-type':
        - error
        - default: array
          readonly: array
    '@typescript-eslint/ban-ts-comment': off # Enable @ts-ignore etc.
    '@typescript-eslint/consistent-type-imports': # Enable import sort order, see bellow.
        - off
        - prefer: type-imports
    '@typescript-eslint/explicit-function-return-type': off # Fix for pinia
    '@typescript-eslint/no-confusing-void-expression': off
    '@typescript-eslint/no-dynamic-delete': off
    '@typescript-eslint/no-extraneous-class': off
    '@typescript-eslint/strict-boolean-expressions': off # Fix for vite import.meta.env
    '@typescript-eslint/triple-slash-reference': off # Fix for vite env.d.ts.
    import/default: off # Fix for Vue setup style
    import/no-default-export: off # Fix for Vuetify
    import/no-named-as-default: off # Fix for Vuetify
    import/no-named-as-default-member: off # Fix for Vuetify
    vue/html-self-closing: # A tag with no content should be written like <br />.
        - error
        - html:
              void: always
    vue/multi-word-component-names: warn # Mitigate non-multiword component name errors to warnings.
    vue/no-template-shadow: off # for Vuetify tooltip fix
    vuetify/no-deprecated-components: warn # for Vuetify Labs Fix (v-data-tables etc.)
settings:
    import/parsers:
        '@typescript-eslint/parser':
            - .ts
            - .tsx
        vue-eslint-parser:
            - .vue
    import/resolver:
        typescript: true
        alias:
            map:
                - ['@', './src']
                - ['~', './node_modules']
            extensions:
                - .js
                - .ts
                - .jsx
                - .tsx
                - .vue
    vite:
        configPath: ./vite.config.ts
