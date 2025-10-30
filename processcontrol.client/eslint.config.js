// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");
const prettier = require("eslint-config-prettier");

/**
 * ESLint Flat Config for Angular + TypeScript + Prettier
 */
module.exports = tseslint.config(
  // ========== TypeScript / Angular files (.ts) ==========
  {
    files: ["**/*.ts"],
    extends: [
      eslint.configs.recommended,
      ...tseslint.configs.recommended,
      ...tseslint.configs.stylistic,
      ...angular.configs.tsRecommended,
      prettier, // disables ESLint rules that conflict with Prettier
    ],
    processor: angular.processInlineTemplates,
    plugins: {
      prettier: require("eslint-plugin-prettier"),
    },
    rules: {
      "prettier/prettier": "warn", // run Prettier as an ESLint rule

      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],
      "@angular-eslint/prefer-standalone": "off",
      "@angular-eslint/prefer-inject": "off",
      "@typescript-eslint/no-explicit-any": ["warn"],
      "no-console": ["warn", { allow: ["warn", "error"] }],
    },
  },

  // ========== Angular Templates (.html) ==========
  {
    files: ["**/*.html"],
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
    ],
    rules: {},
  }
);
